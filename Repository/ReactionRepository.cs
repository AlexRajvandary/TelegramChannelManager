using Contracts;
using Entities.Models;

namespace Repository
{
    public class ReactionRepository : RepositoryBase<Reaction>, IReactionRepository
    {
        public ReactionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Reaction> GetPostReactions(Guid postId, bool trackChanges)
        {
            return FindByCondition(reaction => reaction.PostID == postId, trackChanges);
        }

        public void Update(Reaction reaction) => base.Update(reaction);

        public void Delete(Reaction reaction) => base.Delete(reaction);
    }
}