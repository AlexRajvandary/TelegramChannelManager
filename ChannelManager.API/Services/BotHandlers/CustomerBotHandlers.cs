using ChannelManager.API.Commands;
using Telegram.Bot.Types;
using ChannelManager.API.Extensions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Entities.Exceptions;
using Contracts;
using Telegram.Bot;

namespace ChannelManager.API.Services.BotHandlers
{
    public class CustomerBotHandlers : UpdateHandlers
    {
        private ITelegramBotClient? _botClient;

        public CustomerBotHandlers(ILoggerManager logger,
                                   IServiceManager serviceManager,
                                   ITelegramClientsManager clientsManager) : base(logger, serviceManager, clientsManager)
        {
            _commands = new Dictionary<string, ICommand>
            {
                { typeof(StartMainBotCommand).GetCommandName(), new StartMainBotCommand() },
                { typeof(UsageCommand).GetCommandName(), new UsageCommand() },
                { typeof(CreateNewPostCommand).GetCommandName(), new CreateNewPostCommand() },
                { typeof(AddPostContentCommand).GetCommandName(), new AddPostContentCommand() },
                { typeof(AddPostReactionsCommand).GetCommandName(), new AddPostReactionsCommand() },
                { typeof(AddPostPhotosCommand).GetCommandName(), new AddPostPhotosCommand() }
            };
        }

        public override Task<Message?> BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            return null;
        }

        public override async Task<Message?> BotOnMessageReceived(Message message, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Receive message type: {message.Type}");
            if (message.Text is not { } messageText)
            {
                return null;
            }

            var userDto = _serviceManager.UserService.GetUserByPersonalChatId(message.Chat.Id) ??
                          throw new UserNotFoundException(message.Chat.Id);

            _botClient = await _clientsManager.TryGetOrCreateNewBotClientAsync(userDto.Id, userDto.BotToken, cancellationToken);

            return userDto.State switch
            {
                UserState.None => await ExecuteMainMenuCommand(userDto, messageText, cancellationToken),
                UserState.AwaitingNewPostTitle => await ExecutePostTitleCreationCommand(userDto, messageText, cancellationToken),
                UserState.AwaitingNewPostContent => await ExecutePostContentCreationCommand(userDto, messageText, cancellationToken),
                UserState.AwaitingNewPostReactions => null,
                UserState.AwaitingNewPostPhotos => null,
                UserState.AwaitingToken => null,
                UserState.BotClientCreated => null,
                UserState.AwaitingPostPublishTime => null,
                _ => null,
            };
        }

        private async Task<Message?> ExecuteMainMenuCommand(UserDto userDto, string messageText, CancellationToken cancellationToken)
        {
            if (!_commands.TryGetValue(messageText, out var command))
            {
                await UnknownCommandAsync(messageText, cancellationToken);
                return null;
            }

            var param = await ExecuteCommandAsync(userDto.PersonalChatId, _botClient, command, cancellationToken);
            UpdateUserState(param.UserState, userDto);
            return param.SentMessage;
        }

        private async Task<Message?> ExecutePostTitleCreationCommand(UserDto userDto, string messageText, CancellationToken cancellationToken)
        {
            var newPost = new PostForCreationDto(messageText, null, DateTime.UtcNow);
            _serviceManager.PostService.CreatePost(userDto.Id, newPost, trackChanges: false);

            var param = await ExecuteCommandAsync(userDto.PersonalChatId, _botClient, GetCommand<AddPostContentCommand>(), cancellationToken);
            UpdateUserState(param.UserState, userDto);
            return param.SentMessage;
        }

        private async Task<Message?> ExecutePostContentCreationCommand(UserDto userDto, string messageText, CancellationToken cancellationToken)
        {
            var lastEditedPostId = userDto.LastEditedPostId;
            if (lastEditedPostId is null)
            {
                return null;
            }

            var post = _serviceManager.PostService.GetPost(userDto.Id, lastEditedPostId.Value, false);
            var postForUpdate = new PostForUpdateDto(post.Title, messageText, post.CreatedDate);
            _serviceManager.PostService.UpdatePostForUser(userDto.Id, lastEditedPostId.Value, postForUpdate, false, true);

            var param = await ExecuteCommandAsync(userDto.PersonalChatId, _botClient, GetCommand<AddPostReactionsCommand>(), cancellationToken);
            UpdateUserState(param.UserState, userDto);

            return param.SentMessage;
        }

        private void UpdateUserState(UserState newState, UserDto userDto)
        {
            var userForUpdate = new UserForUpdateDto(userDto.MainChatId, userDto.PersonalChatId, userDto.BotToken, newState, userDto.LastEditedPostId);
            _serviceManager.UserService.UpdateUser(userDto.Id, userForUpdate, true);
        }

        private ICommand GetCommand<T>() where T : ICommand
        {
            return _commands[typeof(T).GetCommandName()];
        }
    }
}