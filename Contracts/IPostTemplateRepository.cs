using Entities.Models;

namespace Contracts
{
    public interface IPostTemplateRepository
    {
        void AddPost(PostTemplate post);

        void DeletePost(PostTemplate post);

        void DeletePost(Guid postId);

        PostTemplate? GetPost(Guid postId);

        IEnumerable<PostTemplate> GetPosts(Guid userId);

        void UpdatePost(PostTemplate post);
    }
}