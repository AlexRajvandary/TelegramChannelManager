using Entities.Models;

namespace Contracts
{
    public interface IPostRepository
    {
        void CreatePostForUser(Guid userId, Post post);

        void DeletePost(Guid postId);

        IEnumerable<Post>? GetPosts(Guid userId, bool trackChanges);

        Post? GetPost(Guid userId, Guid postId, bool trackChanges);

        void UpdatePost(Post post);
    }
}