using Contracts;
using Entities.Models;

namespace Repository
{
    public class ReactionRepository : RepositoryBase<Reaction>, IReactionRepository
    {
        public ReactionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}