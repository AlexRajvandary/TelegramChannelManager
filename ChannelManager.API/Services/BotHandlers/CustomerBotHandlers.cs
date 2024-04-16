using ChannelManager.API.Commands;
using Telegram.Bot.Types;
using ChannelManager.API.Extensions;
using Entities.Models;
using Service.Contracts;
using Microsoft.Extensions.Options;
using Shared.DataTransferObjects;
using Telegram.Bot;

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

            var userDto = _serviceManager.UserService.GetUserByChatId(message.Chat.Id);

            if(userDto == null)
            {
                userDto = _serviceManager.UserService.CreateUser(new UserForCreationDto(message.Chat.Id, null, UserState.None));
            }

            switch (userDto.State)
            {
                case UserState.None:
                    if (_commands.TryGetValue(messageText, out var command))
                    {
                        return await ExecuteCommand(command, cancellationToken);
                    }
                    else
                    {
                        await UnknownCommandAsync(messageText, cancellationToken);
                        return null;
                    }

                case UserState.AwaitingNewPostTitle:
                    var newPost = new PostForCreationDto(messageText, "", DateTime.UtcNow);
                    _serviceManager.PostService.CreatePost(userDto.Id, newPost, trackChanges: false);
                    return await ExecuteCommand(_commands[typeof(AddPostContentCommand).GetCommandName()], cancellationToken);


                case UserState.AwaitingNewPostContent:
                    var lastEditedPost = userDto.LastEditedPost;
                    if (lastEditedPost is null)
                    {
                        return null;
                    }

                    lastEditedPost.Content = messageText;
                    var postForUpdate = new PostForUpdateDto(lastEditedPost.Title, messageText, lastEditedPost.CreatedDate.Value);
                    _serviceManager.PostService.UpdatePostForUser(userDto.Id, lastEditedPost.Id, postForUpdate, false, false);
                    return await ExecuteCommand(_commands[typeof(AddPostReactionsCommand).GetCommandName()], cancellationToken);

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