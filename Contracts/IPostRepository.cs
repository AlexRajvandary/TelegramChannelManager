using Entities.Models;

namespace Contracts
{
    public interface IPostRepository
    {
        void AddPost(Post post);

        void DeletePost(Post post);

        void DeletePost(Guid postId);

        Post? GetPost(Guid postId);

        IEnumerable<Post> GetPosts(Guid userId);

        void UpdatePost(Post post);
    }
}