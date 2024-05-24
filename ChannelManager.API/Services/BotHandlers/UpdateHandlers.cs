using Telegram.Bot.Types;
using ChannelManager.API.Commands;
using Service.Contracts;
using Telegram.Bot;
using Contracts;
using Entities.Models;
using Shared.DataTransferObjects;
using ChannelManager.API.Extensions;
using Entities.Exceptions;

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
                { Message: { } message } => BotOnMessageReceived(message, update.Id, cancellationToken),
                { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, update.Id, cancellationToken),
                _ => UnknownUpdateHandlerAsync(update, cancellationToken)
            };

            await handler;
        }

        public abstract Task<Message?> BotOnMessageReceived(Message message, int updateId, CancellationToken cancellationToken);

        public abstract Task<Message?> BotOnCallbackQueryReceived(CallbackQuery callbackQuery, int updateId, CancellationToken cancellationToken);

        public Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }

        public async Task<Message> UnknownCommandAsync(string commandName, ITelegramBotClient telegramBotClient, ChatId chatId, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Unknown command: {commandName}");
            return await telegramBotClient.SendTextMessageAsync(chatId, "Неизвестная комманда");
        }

        protected void UpdateUserState(UserState newState, UserDto userDto) => UpdateUserState(newState, userDto.LastEditedPostId, userDto.LastUpdateId, userDto);

        protected void UpdateUserState(UserState newState, Guid? newLastEditedPostId, int lastUpdateId, UserDto userDto)
        {
            var lastEditedPostId = newLastEditedPostId ?? userDto.LastEditedPostId;
            var userForUpdate = new UserForUpdateDto(userDto.MainChatId, userDto.PersonalChatId, userDto.BotToken, newState, lastEditedPostId, lastUpdateId);
            _serviceManager.UserService.UpdateUser(userDto.Id, userForUpdate, true);
        }

        protected ICommand GetCommand(Type type)
        {
            if(_commands.TryGetValue(type.GetCommandName(), out var command))
            {
                return command;
            }
            else
            {
                throw new InvalidInputException();
            }
        }

        protected ICommand GetCommand(string commandName)
        {
            return _commands[commandName];
        }
    }
}