using Contracts;
using Entities.Models;

namespace Repository
{
    public class ReactionRepository : RepositoryBase<Reaction>, IReactionRepository
    {
        public ReactionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public IEnumerable<Reaction> GetAllReactions(bool trackChanges) => FindAll(trackChanges)
                                                                           .OrderBy(c => c.Id)
                                                                           .ToList();
    }
}