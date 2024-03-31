using Telegram.Bot.Types;
using ChannelManager.API.Commands;

namespace ChannelManager.API.Services.BotHandlers
{
    public abstract class UpdateHandlers
    {
        protected readonly ILogger<UpdateHandlers> _logger;
        protected readonly IUserContextManager _userContextManager;
        protected Dictionary<string, ICommand> _commands;

        public UpdateHandlers(ILogger<UpdateHandlers> logger,
                              IUserContextManager userContextManager)
        {
            _logger = logger;
            _userContextManager = userContextManager;
        }

        public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            var handler = update switch
            {
                { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
                { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
        }

        public abstract Task<Message?> BotOnMessageReceived(Message message, CancellationToken cancellationToken);

        public abstract Task<Message?> BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken);

        public Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
            return Task.CompletedTask;
        }

        public Task UnknownCommandAsync(string command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unknown command: {command}", command);
            return Task.CompletedTask;
        }
    }
}