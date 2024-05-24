using ChannelManager.API.Commands;
using ChannelManager.API.Commands.CustomerBotCommands;
using ChannelManager.API.Extensions;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChannelManager.API.Services.BotHandlers
{
    public class CustomerBotHandlers : UpdateHandlers
    {
        /// <summary>
        /// Bot client for the current user's conversation.
        /// </summary>
        private ITelegramBotClient? _currentUserBotClient;

        public CustomerBotHandlers(ILoggerManager logger,
                                   IServiceManager serviceManager,
                                   ITelegramClientsManager clientsManager) : base(logger, serviceManager, clientsManager)
        {
            _commands = new Dictionary<string, ICommand>
            {
                { typeof(StartCommand).GetCommandName(), new StartCommand() },
                { typeof(CreateNewPostCommand).GetCommandName(), new CreateNewPostCommand() },
                { typeof(ShowAllPostsCommand).GetCommandName(), new ShowAllPostsCommand() },
                { typeof(AddPostContentCommand).GetCommandName(), new AddPostContentCommand() },
                { typeof(AddPostTitleCommand).GetCommandName(), new AddPostTitleCommand() },
                { typeof(AddPostPhotosCommand).GetCommandName(), new AddPostPhotosCommand() },
                { typeof(SetPostTimeCommand).GetCommandName(), new SetPostTimeCommand() },
                { typeof(GenerateIdeasCommand).GetCommandName(), new GenerateIdeasCommand() },
            };
        }

        public override async Task<Message?> BotOnCallbackQueryReceived(CallbackQuery callbackQuery,
                                                                        int updateId,
                                                                        CancellationToken cancellationToken)
        {
            if(callbackQuery is null)
            {
                _logger.LogWarn($"Callback query is null!");
                return null;
            }

            var userDto = _serviceManager.UserService.GetUserByPersonalChatId(callbackQuery.From.Id) ??
                            throw new UserNotFoundException(callbackQuery.From.Id);

            _currentUserBotClient = await _clientsManager.TryGetOrCreateNewBotClientAsync(userDto.Id, userDto.BotToken, cancellationToken);

            return userDto.State switch
            {
                UserState.None => await ExecuteMainMenuCommand(userDto, 
                                                               callbackQuery.Data,
                                                               updateId,
                                                               cancellationToken),
                _ => null,
            };
        }

        public override async Task<Message?> BotOnMessageReceived(Message message, 
                                                                  int updateId,
                                                                  CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Receive message type: {message.Type}");
            if (message.Text is not { } messageText)
            {
                return null;
            }

            var userDto = _serviceManager.UserService.GetUserByPersonalChatId(message.Chat.Id) ??
                          throw new UserNotFoundException(message.Chat.Id);

            if(userDto.LastUpdateId == updateId)
            {
                return new Message();
            }

            _currentUserBotClient = await _clientsManager.TryGetOrCreateNewBotClientAsync(userDto.Id, userDto.BotToken, cancellationToken);

            return userDto.State switch
            {
                UserState.None => await ExecuteMainMenuCommand(userDto, messageText, updateId, cancellationToken),
                UserState.AwaitingNewPostTitle => await ExecutePostTitleCreationCommand(userDto, messageText, cancellationToken),
                UserState.AwaitingNewPostContent => await ExecutePostContentCreationCommand(userDto, messageText, cancellationToken),
                UserState.AwaitingNewPostReactions => null,
                UserState.AwaitingNewPostPhotos => null,
                UserState.AwaitingPostPublishTime => null,
                UserState.AwaitingPostIdeasPromt => null,
                _ => null,
            };
        }

        private async Task<Message?> ExecuteMainMenuCommand(UserDto userDto, 
                                                            string messageText,
                                                            int updateId,
                                                            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(_currentUserBotClient));
            ArgumentNullException.ThrowIfNull(nameof(userDto));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(messageText));

            if(!CommandRequest.TryParse(messageText, _commands, out var commandRequest))
            {
                UpdateUserState(userDto.State, userDto.LastEditedPostId, userDto.LastUpdateId, userDto);
                await UnknownCommandAsync(messageText, _currentUserBotClient, userDto.PersonalChatId, cancellationToken);
                throw new InvalidInputException();
            }

            ExecutedCommandParapms param;
            Guid? lastEditedPostId = null;

            if (commandRequest.Command is ShowAllPostsCommand showAllPostsCommand)
            {
                showAllPostsCommand.Posts = _serviceManager.PostService.GetPosts(userDto.Id, false)?.ToList();
                param = await showAllPostsCommand.ExecuteAsync(_currentUserBotClient, userDto.PersonalChatId, cancellationToken);
            }
            else if(commandRequest.Command is ShowPostCommand showPostCommand)
            {
                var postId = commandRequest.CommandParameter.Parameter;
                showPostCommand.Post = _serviceManager.PostService.GetPost(userDto.Id, postId, trackChanges: false) ?? throw new PostNotFoundException(postId);
                param = await showPostCommand.ExecuteAsync(_currentUserBotClient, userDto.PersonalChatId, cancellationToken);
            }
            else if(commandRequest.Command is CreateNewPostCommand createNewPostCommand)
            {
                var postForCreationDto = PostForCreationDto.CreateNewInstance();
                //Id поста должен генерироваться сам и здесь мы должны его получать и сохранять для дальнейшего использования
                var postDto = _serviceManager.PostService.CreatePost(userDto.Id, postForCreationDto, trackChanges: false);
                lastEditedPostId = postDto.Id;
                param = await createNewPostCommand.ExecuteAsync(_currentUserBotClient, userDto.PersonalChatId, cancellationToken);
            }
            else
            {
                param = await commandRequest.Command.ExecuteAsync(_currentUserBotClient, userDto.PersonalChatId, cancellationToken);
            }

            UpdateUserState(param.UserState, lastEditedPostId, updateId, userDto);
            return param.SentMessage;

        }

        private async Task<Message?> ExecutePostTitleCreationCommand(UserDto userDto, string messageText, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(_currentUserBotClient));
            var command = GetCommand(typeof(AddPostTitleCommand));
            var param = await command.ExecuteAsync(_currentUserBotClient, userDto.PersonalChatId, cancellationToken);
            UpdateUserState(param.UserState, userDto);

            return param.SentMessage;
        }

        private async Task<Message?> ExecutePostContentCreationCommand(UserDto userDto, string messageText, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(nameof(_currentUserBotClient));
            ArgumentNullException.ThrowIfNull(nameof(userDto.LastEditedPostId));

            var post = _serviceManager.PostService.GetPost(userDto.Id, userDto.LastEditedPostId.Value, false);
            var postForUpdate = new PostForUpdateDto(post.Title, messageText, post.CreatedDate);
            _serviceManager.PostService.UpdatePostForUser(userDto.Id, userDto.LastEditedPostId.Value, postForUpdate, false, true);

            var command = GetCommand(typeof(AddPostReactionsCommand));
            var param = await command.ExecuteAsync(_currentUserBotClient, userDto.PersonalChatId, cancellationToken);
            UpdateUserState(param.UserState, userDto);

            return param.SentMessage;
        }
    }
}