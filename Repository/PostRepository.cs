using Contracts;
using Entities.Models;

namespace Repository
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
        
        public void AddPost(Post post)
        {
            ArgumentNullException.ThrowIfNull(nameof(post));
            Create(post);
        }

        public Post? GetPost(Guid postId) => RepositoryContext.Posts?.Find(postId);

        public IEnumerable<Post> GetPosts(Guid userId) => FindByCondition(post => post.UserId == userId, true) ?? Enumerable.Empty<Post>();

        public void DeletePost(Guid postId) => base.Delete(GetPost(postId));

        public void DeletePost(Post post) => base.Delete(post);

        public void UpdatePost(Post post) => base.Update(post);
    }
}