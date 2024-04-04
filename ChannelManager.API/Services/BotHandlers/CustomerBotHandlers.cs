using ChannelManager.API.Commands;
using Telegram.Bot.Types;
using ChannelManager.API.Extensions;
using Entities.Models;
using Service.Contracts;
using Microsoft.Extensions.Options;

namespace ChannelManager.API.Services.BotHandlers
{
    public class CustomerBotHandlers : UpdateHandlers
    {
        public CustomerBotHandlers(ILogger<UpdateHandlers> logger,
                                   IOptions<BotConfiguration> botOptions,
                                   IUserContextManager userContextManager,
                                   IServiceManager serviceManager) : base(logger, botOptions, userContextManager, serviceManager)
        {
            _webhookAddress += "/customerBot";
            _commands = new Dictionary<string, ICommand>
            {
                { typeof(StartCommand).GetCommandName(), new StartCommand() },
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

            if (!_userContextManager.TryGetUserContext(message.Chat.Id, out var userContext))
            {
                var user = _serviceManager.UserService.GetUserByChatId(message.Chat.Id);

                if (user is not null)
                {
                    userContext = await _userContextManager.RestoreUserContextAsync(user, _webhookAddress);
                }
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
                    if (lastEditedPost is null)
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