using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IPostService
    {
        PostDto CreatePost(Guid userId, PostForCreationDto post, bool trackChanges);

        void DeletePost(Guid userId, Guid postId);

        void UpdatePostForUser(Guid userId, Guid postId, PostForUpdateDto postForUpdate, bool userTrackChanges, bool postTrackChanges);

        PostDto? GetPost(Guid userId, Guid postId, bool trackChanges);

        IEnumerable<PostDto>? GetPosts(Guid userId, bool trackChanges);
    }
}
