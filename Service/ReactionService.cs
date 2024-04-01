using Contracts;
using Entities.Models;
using Service.Contracts;

namespace Service
{
    public class ReactionService : IReactionService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ReactionService(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IEnumerable<Reaction> GetPostReactions(Guid postId)
        {
            throw new NotImplementedException();
        }

        public void SetPostReactions(Guid postId, IEnumerable<Reaction> reactions)
        {
            throw new NotImplementedException();
        }
    }
}
