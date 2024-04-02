using ChannelManager.API.Commands;
using Telegram.Bot.Types;
using ChannelManager.API.Extensions;
using Entities.Models;
using Service.Contracts;

namespace ChannelManager.API.Services.BotHandlers
{
    public class CustomerBotHandlers : UpdateHandlers
    {
        public CustomerBotHandlers(ILogger<UpdateHandlers> logger,
                                   IUserContextManager userContextManager,
                                   IServiceManager serviceManager) : base(logger, userContextManager, serviceManager)
        {
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

            if (!_userContextManager.TryGetUserContext(message.Chat.Id, out var userContext))
            {
                userContext = _userContextManager.CreateNewUserContext(message.Chat.Id);
            }

            switch (userContext.State)
            {
                case UserState.None:
                    if (_commands.TryGetValue(messageText, out var command))
                    {
                        return await userContext.ExecuteCommand(command, cancellationToken);
                    }
                    else
                    {
                        await UnknownCommandAsync(messageText, cancellationToken);
                        return null;
                    }

                case UserState.AwaitingNewPostTitle:
                    var newPost = new Post() { Title = messageText, CreatedDate = DateTime.UtcNow, UserId = userContext.UserId };
                    userContext.LastEditedPost = newPost;
                    _serviceManager.PostService.CreatePost(newPost);
                    return await userContext.ExecuteCommand(_commands[typeof(AddPostContentCommand).GetCommandName()], cancellationToken);
                   

                case UserState.AwaitingNewPostContent:
                    var lastEditedPost = userContext.LastEditedPost;
                    if(lastEditedPost is null)
                    {
                        return null;
                    }

                    lastEditedPost.Content = messageText;
                    _serviceManager.PostService.UpdatePost(lastEditedPost);
                    return await userContext.ExecuteCommand(_commands[typeof(AddPostReactionsCommand).GetCommandName()], cancellationToken);

                case UserState.AwaitingNewPostReactions:
                    lastEditedPost = userContext.LastEditedPost;
                    if (lastEditedPost is null)
                    {
                        return null;
                    }

                    lastEditedPost.Reactions = [];
                    _serviceManager.PostService.UpdatePost(lastEditedPost);
                    return await userContext.ExecuteCommand(_commands[typeof(AddPostReactionsCommand).GetCommandName()], cancellationToken);

                case UserState.AwaitingNewPostPhotos:

                    break;
            }

            return null;
        }
    }
}