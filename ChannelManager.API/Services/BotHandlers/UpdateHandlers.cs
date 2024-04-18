using Telegram.Bot.Types;
using ChannelManager.API.Commands;
using Service.Contracts;
using Telegram.Bot;
using Contracts;
using Entities.Models;
using Shared.DataTransferObjects;
using ChannelManager.API.Extensions;

namespace ChannelManager.API.Services.BotHandlers
{
    public abstract class UpdateHandlers
    {
        protected readonly ILoggerManager _logger;
        protected readonly IServiceManager _serviceManager;
        protected readonly ITelegramClientsManager _clientsManager;

        protected Dictionary<string, ICommand> _commands;
        
        public UpdateHandlers(ILoggerManager logger,
                              IServiceManager serviceManager,
                              ITelegramClientsManager clientsManager)
        {
            _logger = logger;
            _serviceManager = serviceManager;
            _clientsManager = clientsManager;
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
            _logger.LogInfo($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        public Task UnknownCommandAsync(string command, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Unknown command: {command}");
            return Task.CompletedTask;
        }

        public async Task<ExecutedCommandParapms> ExecuteCommandAsync(long chatId, ITelegramBotClient? telegramBotClient, ICommand command, CancellationToken cancellationToken)
        {
            if (telegramBotClient == null)
            {
                _logger.LogError($"{nameof(telegramBotClient)} is null");
                return null;
            }

            return await command.ExecuteAsync(telegramBotClient, chatId, cancellationToken);
        }

        protected void UpdateUserState(UserState newState, UserDto userDto)
        {
            var userForUpdate = new UserForUpdateDto(userDto.MainChatId, userDto.PersonalChatId, userDto.BotToken, newState, userDto.LastEditedPostId);
            _serviceManager.UserService.UpdateUser(userDto.Id, userForUpdate, true);
        }

        protected ICommand GetCommand<T>() where T : ICommand
        {
            return _commands[typeof(T).GetCommandName()];
        }
    }
}