using ChannelManager.API.Commands;
using Telegram.Bot.Types;
using ChannelManager.API.Extensions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Entities.Exceptions;

namespace ChannelManager.API.Services.BotHandlers
{
    public class CustomerBotHandlers : UpdateHandlers
    {
        public CustomerBotHandlers(ILogger<UpdateHandlers> logger,
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
            _logger.LogInformation("Receive message type: {MessageType}", message.Type);
            if (message.Text is not { } messageText)
            {
                return null;
            }

            var userDto = _serviceManager.UserService.GetUserByPersonalChatId(message.Chat.Id) ??
                          throw new UserNotFoundException(message.Chat.Id);

            var telegramBotClient = _clientsManager.GetBotClient(userDto.Id);

            switch (userDto.State)
            {
                case UserState.None:
                    if (!_commands.TryGetValue(messageText, out var command))
                    {
                        await UnknownCommandAsync(messageText, cancellationToken);
                        return null;
                    }
                    
                    var param = await ExecuteCommandAsync(message.Chat.Id, telegramBotClient, command, cancellationToken);

                    var userForUpdate = new UserForUpdateDto(userDto.MainChatId, userDto.PersonalChatId, userDto.BotToken, param.UserState, userDto.LastEditedPost);
                    _serviceManager.UserService.UpdateUser(userDto.Id, userForUpdate, true);
                    return param.SentMessage;

                case UserState.AwaitingNewPostTitle:
                    var newPost = new PostForCreationDto(messageText, null, DateTime.UtcNow);
                    _serviceManager.PostService.CreatePost(userDto.Id, newPost, trackChanges: false);

                    var sentMessageParams = await ExecuteCommandAsync(message.Chat.Id, telegramBotClient, _commands[typeof(AddPostContentCommand).GetCommandName()], cancellationToken);

                    userForUpdate = new UserForUpdateDto(userDto.MainChatId, userDto.PersonalChatId, userDto.BotToken, sentMessageParams.UserState, userDto.LastEditedPost);
                    _serviceManager.UserService.UpdateUser(userDto.Id, userForUpdate, true);
                    return sentMessageParams.SentMessage;

                case UserState.AwaitingNewPostContent:
                    var lastEditedPostId = userDto.LastEditedPost;
                    if (lastEditedPostId is null)
                    {
                        return null;
                    }

                    var post = _serviceManager.PostService.GetPost(userDto.Id, lastEditedPostId.Value, false);
                    var postForUpdate = new PostForUpdateDto(post.Title, messageText, post.CreatedDate);
                    _serviceManager.PostService.UpdatePostForUser(userDto.Id, lastEditedPostId.Value, postForUpdate, false, true);

                    sentMessageParams = await ExecuteCommandAsync(message.Chat.Id, telegramBotClient, _commands[typeof(AddPostReactionsCommand).GetCommandName()], cancellationToken);

                    userForUpdate = new UserForUpdateDto(userDto.MainChatId, userDto.PersonalChatId, userDto.BotToken, sentMessageParams.UserState, userDto.LastEditedPost);
                    _serviceManager.UserService.UpdateUser(userDto.Id, userForUpdate, true);
                    return sentMessageParams.SentMessage;

                case UserState.AwaitingNewPostReactions:
                    //lastEditedPost = userContext.LastEditedPost;
                    //if (lastEditedPost is null)
                    //{
                    //    return null;
                    //}

                    //lastEditedPost.Reactions = [];
                    //_serviceManager.PostService.UpdatePost(lastEditedPost);
                    //return await userContext.ExecuteCommand(_commands[typeof(AddPostReactionsCommand).GetCommandName()], cancellationToken);
                    break;

                case UserState.AwaitingNewPostPhotos:

                    break;
            }

            return null;
        }
    }
}