using Contracts;
using Entities.Models;

namespace Repository
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreatePostForUser(Guid userId, Post post)
        {
            post.UserId = userId;
            Create(post);
        }

        public Post? GetPost(Guid userId, Guid postId, bool trackChanges)
        {
            return FindByCondition(e => e.UserId.Equals(userId) && e.Id.Equals(postId), trackChanges).SingleOrDefault();
        }

        public IEnumerable<Post> GetPosts(Guid userId, bool trackChanges)
        {
            return FindByCondition(e => e.UserId.Equals(userId), trackChanges).ToList();
        }

        public void DeletePost(Guid postId) => base.Delete(GetPost(postId));

        public void UpdatePost(Post post) => base.Update(post);
    }
}