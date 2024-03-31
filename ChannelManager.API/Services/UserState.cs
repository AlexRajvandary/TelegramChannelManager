namespace ChannelManager.API.Services
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