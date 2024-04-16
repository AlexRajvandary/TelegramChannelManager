namespace Entities.Exceptions
{
    public sealed class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException(Guid postId)
            : base($"Post with id: {postId} doesn't exist in the database.")
        {
        }
    }
}
