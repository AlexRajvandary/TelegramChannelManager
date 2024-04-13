using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IPostService
    {
        void CreatePost(PostDTO post);

        void DeletePost(PostDTO post);

        void UpdatePost(PostDTO post);

        PostDTO GetPostById(Guid postId);
    }
}
