using Entities.Models;

namespace Service.Contracts
{
    public interface IReactionService
    {
        IEnumerable<Reaction> GetAllReactions(bool trackChanges);
    }
}
