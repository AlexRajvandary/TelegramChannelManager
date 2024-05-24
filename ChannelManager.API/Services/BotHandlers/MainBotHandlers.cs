using ChannelManager.API.Commands;
using ChannelManager.API.Commands.MainBotCommands;
using ChannelManager.API.Extensions;
using Contracts;
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
        /// <summary>
        /// Main bot client for all users.
        /// </summary>
        private readonly ITelegramBotClient _mainBotClient;

        public MainBotHandlers(ITelegramBotClient botClient,
                               ILoggerManager logger,
                               ITelegramClientsManager clientsManager,
                               IServiceManager serviceManager) : base(logger, serviceManager, clientsManager)
        {
            _mainBotClient = botClient;

            _commands = new Dictionary<string, ICommand>
            {
                { "/start", new StartMainBotCommand() },
                { typeof(IncorrectTokenCommand).GetCommandName(), new IncorrectTokenCommand() },
                { typeof(BotWasSuccessfullyCreatedCommand).GetCommandName(), new BotWasSuccessfullyCreatedCommand() },
            };
        }

        public override async Task<Message?> BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            return await UnknownCommandAsync(callbackQuery.Data, _mainBotClient, callbackQuery.From.Id, cancellationToken);
        }

        public override async Task<Message?> BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Receive message type: {message.Type}");
            var command = GetCommand("/start");
            if (message.Text is not { } messageText)
            {
                return null;
            }

            var userDto = _serviceManager.UserService.TryGetUserByPersonalChatId(message.Chat.Id);

            if (userDto is null)
            {
                var userForCreationDto = new UserForCreationDto(message.Chat.Id, message.Chat.Id, null, UserState.AwaitingToken, null);
                _serviceManager.UserService.CreateUser(userForCreationDto);
                return (await command.ExecuteAsync(_mainBotClient, message.Chat.Id, cancellationToken)).SentMessage;
            }

            return userDto.State switch
            {
                UserState.None => await ExecuteMainMenuCommand(userDto, messageText, cancellationToken),
                UserState.AwaitingToken => await ExecuteCheckTokenCommand(userDto, messageText, cancellationToken),
                _ => null,
            };
        }

        private async Task<Message?> ExecuteMainMenuCommand(UserDto userDto, string messageText, CancellationToken cancellationToken)
        {
            if (!_commands.TryGetValue(messageText, out var command))
            {
                return await UnknownCommandAsync(messageText, _mainBotClient, userDto.PersonalChatId, cancellationToken);
            }

            var param = await command.ExecuteAsync(_mainBotClient, userDto.PersonalChatId, cancellationToken);
            UpdateUserState(param.UserState, userDto);
            return param.SentMessage;
        }

        private async Task<Message?> ExecuteCheckTokenCommand(UserDto userDto, string messageText, CancellationToken cancellationToken)
        {
            ExecutedCommandParapms param;
            UserForUpdateDto userForUpdate;

            if (!IsCorrectTelegramBotToken(messageText))
            {
                var command = GetCommand(typeof(IncorrectTokenCommand));
                param = await command.ExecuteAsync(_mainBotClient, userDto.MainChatId, cancellationToken);
                UpdateUserState(param.UserState, userDto);
                return param.SentMessage;
            }

            _ = await _clientsManager.TryGetOrCreateNewBotClientAsync(userDto.Id, messageText, cancellationToken);

            var botWasCreatedCommand = GetCommand(typeof(BotWasSuccessfullyCreatedCommand));

            param = await botWasCreatedCommand.ExecuteAsync(_mainBotClient,userDto.MainChatId, cancellationToken);
            userForUpdate = new UserForUpdateDto(userDto.MainChatId, userDto.PersonalChatId, messageText, param.UserState, userDto.LastEditedPostId);
            _serviceManager.UserService.UpdateUser(userDto.Id, userForUpdate, true);

            return param.SentMessage;
        }

        private bool IsCorrectTelegramBotToken(string token)
        {
            var reg = new Regex(@"^\d{10}:[\w\d]*$");
            return reg.IsMatch(token);
        }
    }
}