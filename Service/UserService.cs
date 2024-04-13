using AutoMapper;
using Contracts;
using Service.Contracts;
using Shared.DataTransferObjects;
using Telegram.Bot.Types;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public UserService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public void AddUser(UserDto user)
        {
            _repository.UserRepository.CreateUser(user);
            _logger.LogInfo($"User with chat Id: [{user.ChatId}] is created.");
            _repository.Save();
        }

        public UserDto? GetUser(Guid Id)
        {
            var user = _repository.UserRepository.GetUser(Id, trackChanges: false);
            _logger.LogInfo($"User with Id: [{Id}] was retrieved.");
            var userDTO = _mapper.Map<UserDto>(user);
            return userDTO;
        }

        public UserDto? GetUserByChatId(long chatId)
        {
            var user = _repository.UserRepository.GetUser(chatId, trackChanges: false);
            _logger.LogInfo($"User with chat Id: [{chatId}] was retrieved.");
            var userDTO = _mapper.Map<UserDto>(user);
            return userDTO;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _repository.UserRepository.GetAllUsers();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return usersDto;
        }

        public void UpdateUser(UserDto user)
        {
            _logger.LogInfo($"User with chat Id: [{user.ChatId}] was updated.");
            _repository.UserRepository.UpdateUser(user);
            _repository.Save();
        }
    }
}