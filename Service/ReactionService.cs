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

        public IEnumerable<Reaction> GetAllReactions(bool trackChanges)
        {
            try
            {
                var companies = _repository.ReactionRepository.GetAllReactions(trackChanges);
                return companies;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetAllReactions)} service method {ex}");
                throw;
            }
        }
    }
}
