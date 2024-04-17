using ChannelManager.API.Commands;
using ChannelManager.API.Extensions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChannelManager.API.Services.BotHandlers
{
    public class MainBotHandlers : UpdateHandlers
    {
        private ITelegramBotClient _botClient;

        public MainBotHandlers(ITelegramBotClient botClient,
                               ILogger<UpdateHandlers> logger,
                               ITelegramClientsManager clientsManager,
                               IServiceManager serviceManager) : base(logger, serviceManager, clientsManager)
        {
            _botClient = botClient;

            _commands = new Dictionary<string, ICommand>
            {
                { typeof(StartMainBotCommand).GetCommandName(), new StartMainBotCommand() },
                { typeof(IncorrectTokenCommand).GetCommandName(), new IncorrectTokenCommand() },
                { typeof(BotWasSuccessfullyCreatedCommand).GetCommandName(), new BotWasSuccessfullyCreatedCommand() },
            };
        }

        public override async Task<Message?> BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            await UnknownCommandAsync("Callback", cancellationToken);
            return null;
        }

        public override async Task<Message?> BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Text is not { } messageText)
            {
                return null;
            }

            var userDto = _serviceManager.UserService.GetUserByPersonalChatId(message.Chat.Id);

            if (userDto is null)
            {
                var userForCreationDto = new UserForCreationDto(message.Chat.Id, message.Chat.Id, null, UserState.AwaitingToken, null);
                _serviceManager.UserService.CreateUser(userForCreationDto);
                return (await ExecuteCommandAsync(message.Chat.Id, _botClient, _commands[typeof(StartMainBotCommand).GetCommandName()], cancellationToken)).SentMessage;
            }

            switch (userDto.State)
            {
                case UserState.None:
                    if (_commands.TryGetValue(messageText, out var command))
                    {
                        var sentMessageParams = await ExecuteCommandAsync(message.Chat.Id, _botClient, command, cancellationToken);
                        return sentMessageParams.SentMessage;
                    }
                    else
                    {
                        await UnknownCommandAsync(messageText, cancellationToken);
                        return null;
                    }

                case UserState.AwaitingToken:

                    ExecutedCommandParapms param;

                    if (IsCorrectTelegramBotToken(messageText))
                    {
                        var customerBotClient = await _clientsManager.CreateNewBotClientAsync(userDto.Id, messageText, cancellationToken);

                        if (customerBotClient is not null)
                        {
                            param = await ExecuteCommandAsync(message.Chat.Id, _botClient, _commands[typeof(BotWasSuccessfullyCreatedCommand).GetCommandName()], cancellationToken);
                        }
                        else
                        {
                            param = new ExecutedCommandParapms(null, UserState.AwaitingToken);
                        }
                    }
                    else
                    {
                        param = await ExecuteCommandAsync(message.Chat.Id, _botClient, _commands[typeof(IncorrectTokenCommand).GetCommandName()], cancellationToken);
                    }

                    var userForUpdate = new UserForUpdateDto(userDto.MainChatId, userDto.PersonalChatId, messageText, UserState.None, null);
                    _serviceManager.UserService.UpdateUser(userDto.Id, userForUpdate, trackChanges: true);

                    return param.SentMessage;
            }

            return null;
        }

        private bool IsCorrectTelegramBotToken(string token)
        {
            var reg = new Regex(@"^\d{10}:[\w\d]*$");
            return reg.IsMatch(token);
        }
    }
}