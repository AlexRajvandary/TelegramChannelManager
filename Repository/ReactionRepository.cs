using Contracts;
using Entities.Models;

namespace Repository
{
    public class ReactionRepository : RepositoryBase<Reaction>, IReactionRepository
    {
        public ReactionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Reaction> GetPostReactions(Guid postId, bool trackChanges) => FindByCondition(reaction => reaction.PostID == postId, trackChanges);

        public void SetPostReactions(Guid postId, IEnumerable<Reaction> postReactions)
        {
            throw new NotImplementedException();
        }
    }
}