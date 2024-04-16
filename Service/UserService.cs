using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

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

        public UserDto CreateUser(UserForCreationDto user)
        {
            var userEntity = _mapper.Map<User>(user);

            _repository.UserRepository.CreateUser(userEntity);
            _repository.Save();
            _logger.LogInfo($"User with chat Id: [{user.ChatId}] is created.");

            var userToReturn = _mapper.Map<UserDto>(user);
            return userToReturn;
        }

        public UserDto? GetUser(Guid id)
        {
            var userEntity = _repository.UserRepository.GetUser(id, trackChanges: false);
            if (userEntity == null)
            {
                throw new UserNotFoundException(id);
            }

            _logger.LogInfo($"User with Id: [{id}] was retrieved.");
            var userDTO = _mapper.Map<UserDto>(userEntity);
            return userDTO;
        }

        public UserDto? GetUserByChatId(long chatId)
        {
            var userEntity = _repository.UserRepository.GetUser(chatId, trackChanges: false);
            if (userEntity == null)
            {
                throw new UserNotFoundException(chatId);
            }

            _logger.LogInfo($"User with chat Id: [{chatId}] was retrieved.");
            var userDTO = _mapper.Map<UserDto>(userEntity);
            return userDTO;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var userEntities = _repository.UserRepository.GetAllUsers();
            _logger.LogInfo($"Collection of all users was retrieved.");
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(userEntities);
            return usersDto;
        }

        public void UpdateUser(Guid id, UserForUpdateDto userForUpdate, bool trackChanges)
        {
            var userEntity = _repository.UserRepository.GetUser(id, trackChanges);
            if(userEntity == null)
            {
                throw new UserNotFoundException(id);
            }

            _mapper.Map(userForUpdate, userEntity);
            _repository.Save();
            _logger.LogInfo($"User with chat Id: [{userForUpdate.ChatId}] was successfully updated.");
        }
    }
}