using Shared.DataTransferObjects;

namespace Service.Contracts
{
    public interface IUserService
    {
        void AddUser(UserDto user);

        UserDto? GetUserByChatId(long chatId);

        UserDto? GetUser(Guid Id);

        IEnumerable<UserDto> GetUsers();

        void UpdateUser(UserDto user);
    }
}
