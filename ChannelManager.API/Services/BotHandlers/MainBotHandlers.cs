using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using ChannelManager.API.Commands;
using ChannelManager.API.Extensions;
using Service.Contracts;
using Entities.Models;

namespace ChannelManager.API.Services.BotHandlers
{
    public class MainBotHandlers : UpdateHandlers
    {
        private ITelegramBotClient _botClient;

        public MainBotHandlers(ITelegramBotClient botClient,
                               IOptions<BotConfiguration> botOptions,
                               ILogger<UpdateHandlers> logger,
                               IUserContextManager userContextManager,
                               IServiceManager serviceManager) : base(logger, botOptions, userContextManager, serviceManager)
        {
            _botClient = botClient;
          
            _commands = new Dictionary<string, ICommand>
            {
                { typeof(StartCommand).GetCommandName(), new StartCommand() },
                { typeof(UsageCommand).GetCommandName(), new UsageCommand() },
                { typeof(IncorrectTokenCommand).GetCommandName(), new IncorrectTokenCommand() }
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

            if (!_userContextManager.TryGetUserContext(message.Chat.Id, out var userContext))
            {
                var user = _serviceManager.UserService.GetUserByChatId(message.Chat.Id);

                if (user is not null)
                {
                    userContext = await _userContextManager.RestoreUserContextAsync(user, _webhookAddress);
                }
                else
                {
                    _serviceManager.UserService.AddUser(new Entities.Models.User() { ChatId = message.Chat.Id, State = UserState.None });
                    userContext = _userContextManager.CreateNewUserContext(message.Chat.Id);
                }
            }

            switch (userContext.State)
            {
                case UserState.None:
                    if (_commands.TryGetValue(messageText, out var command))
                    {
                        return await userContext.ExecuteCommand(_botClient, command, cancellationToken);
                    }
                    else
                    {
                        await UnknownCommandAsync(messageText, cancellationToken);
                        return null;
                    }

                case UserState.AwaitingToken:
                    if (IsCorrectTelegramBotToken(messageText))
                    {
                        await userContext.CreateTelegramClientAsync(messageText, _webhookAddress + "/customerBot");
                    }
                    else
                    {
                        await userContext.ExecuteCommand(_botClient, _commands[typeof(IncorrectTokenCommand).GetCommandName()], cancellationToken);
                    }
                    return null;

                case UserState.BotClientCreated:
                    if (_commands.TryGetValue(messageText, out command) && command is not StartCommand)
                    {
                        return await userContext.ExecuteCommand(_botClient, command, cancellationToken);
                    }
                    else if (command is StartCommand)
                    {
                        return await userContext.ExecuteCommand(_botClient, _commands[typeof(UsageCommand).GetCommandName()], cancellationToken);
                    }
                    else
                    {
                        await UnknownCommandAsync(messageText, cancellationToken);
                        return null;
                    }
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