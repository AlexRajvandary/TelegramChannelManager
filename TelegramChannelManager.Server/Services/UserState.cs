namespace TelegramChannelManager.Server.Services
{
    public enum UserState
    {
        None,
        AwaitingToken,
        BotClientCreated,
        AwaitingNewPostTitle,
        AwaitingNewPostContent,
        AwaitingNewPostReactions,
        AwaitingNewPostPhotos
    }
}