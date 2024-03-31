using Contracts;
using Entities.Models;

namespace Repository
{
    public class PostTemplateRepository : RepositoryBase<PostTemplate>, IPostTemplateRepository
    {
        public PostTemplateRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void AddPost(PostTemplate postTemplate)
        {
            ArgumentNullException.ThrowIfNull(nameof(postTemplate));
            Create(postTemplate);
        }

        public PostTemplate? GetPost(Guid postId) => RepositoryContext.PostTemplates?.Find(postId);

        public IEnumerable<PostTemplate> GetPosts(Guid userId) => FindByCondition(post => post.UserId == userId, true) ?? Enumerable.Empty<PostTemplate>();

        public void DeletePost(Guid postId) => base.Delete(GetPost(postId));

        public void DeletePost(PostTemplate post) => base.Delete(post);

        public void UpdatePost(PostTemplate post) => base.Update(post);
    }
}