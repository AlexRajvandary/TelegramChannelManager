using Telegram.Bot.Types;
using TelegramChannelManager.Server.Commands;
using TelegramChannelManager.Server.Extensions;

namespace TelegramChannelManager.Server.Services.BotHandlers
{
    public class CustomerBotHandlers : UpdateHandlers
    {
        public CustomerBotHandlers(ILogger<UpdateHandlers> logger,
                                   IUserContextManager userContextManager) : base(logger, userContextManager)
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
                    await userContext.AddNewPost(new Models.Post(messageText));
                    return await userContext.ExecuteCommand(_commands[typeof(AddPostContentCommand).GetCommandName()], cancellationToken);
                   

                case UserState.AwaitingNewPostContent:

                    break;

                case UserState.AwaitingNewPostReactions:

                    break;

                case UserState.AwaitingNewPostPhotos:

                    break;
            }

            return null;
        }
    }
}