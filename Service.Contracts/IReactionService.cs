using Entities.Models;

namespace Service.Contracts
{
    public interface IReactionService
    {
        IEnumerable<Reaction> GetPostReactions(Guid postId);

        void SetPostReactions(Guid postId, IEnumerable<Reaction> reactions);
    }
}
