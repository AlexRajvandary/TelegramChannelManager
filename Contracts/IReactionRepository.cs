using Entities.Models;

namespace Contracts
{
    public interface IReactionRepository
    {
        IEnumerable<Reaction> GetAllReactions(bool trackChanges);

        IEnumerable<Reaction> GetReactions(Guid postId);
    }
}