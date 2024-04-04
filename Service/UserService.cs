using Contracts;
using Entities.Models;
using Service.Contracts;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public UserService(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public void AddUser(User user)
        {
            _repository.UserRepository.CreateUser(user);
            _logger.LogInfo($"User with chat Id: [{user.ChatId}] is created.");
        }

        public User? GetUserByChatId(long chatId)
        {
            _logger.LogInfo($"User with chat Id: [{chatId}] was retrieved.");
            return _repository.UserRepository.GetUser(chatId, trackChanges: false);
        }

        public void UpdateUser(User user)
        {
            _logger.LogInfo($"User with chat Id: [{user.ChatId}] was updated.");
            _repository.UserRepository.UpdateUser(user);
        }
    }
}