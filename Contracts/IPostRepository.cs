using Entities.Models;

namespace Contracts
{
    public interface IPostRepository
    {
        Post GetPost(Guid postId);

        IEnumerable<Post> GetPosts(Guid userId);
    }
}