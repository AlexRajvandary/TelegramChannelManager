using Entities.Models;

namespace Service.Contracts
{
    public interface IPostService
    {
        void CreatePost(Post post);

        void DeletePost(Post post);

        void UpdatePost(Post post);

        Post GetPostById(Guid postId);
    }
}
