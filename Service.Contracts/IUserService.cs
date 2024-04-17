using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        UserDto CreateUser(UserForCreationDto userForCreation);

        UserDto GetUserByPersonalChatId(long chatId);

        UserDto GetUserByMainChatId(long chatId);

        UserDto GetUser(Guid id);

        UserDto? TryGetUserByPersonalChatId(long chatId);

        UserDto? TryGetUserByMainChatId(long chatId);

        UserDto? TryGetUser(Guid id);

        IEnumerable<UserDto> GetUsers();

        void UpdateUser(Guid id, UserForUpdateDto userForUpdate, bool trackChanges);
    }
}
