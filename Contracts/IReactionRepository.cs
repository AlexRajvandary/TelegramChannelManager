using Entities.Models;

namespace Contracts
{
    public interface IReactionRepository
    {
        IEnumerable<Reaction> GetPostReactions(Guid postId, bool trackChanges);

        void SetPostReactions(Guid postId, IEnumerable<Reaction> postReactions);
    }
}