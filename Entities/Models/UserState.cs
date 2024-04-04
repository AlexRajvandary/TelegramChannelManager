namespace Entities.Models
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