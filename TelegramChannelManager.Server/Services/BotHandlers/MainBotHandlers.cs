using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChannelManager.Server.Commands;
using TelegramChannelManager.Server.Extensions;

namespace TelegramChannelManager.Server.Services.BotHandlers
{
    public class MainBotHandlers : UpdateHandlers
    {
        private ITelegramBotClient _botClient;
        private string webhookAddress;

        public MainBotHandlers(ITelegramBotClient botClient,
                               IOptions<BotConfiguration> botOptions,
                               ILogger<UpdateHandlers> logger,
                               IUserContextManager userContextManager) : base(logger, userContextManager)
        {
            _botClient = botClient;
            webhookAddress = botOptions.Value.HostAddress + "/customerBot";

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
                userContext = _userContextManager.CreateNewUserContext(message.Chat.Id);
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
                        await userContext.CreateTelegramClientAsync(messageText, webhookAddress);
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