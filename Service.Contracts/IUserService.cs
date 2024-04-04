using Entities.Models;

namespace Service.Contracts
{
    public interface IUserService
    {
        void AddUser(User user);
        User? GetUserByChatId(long chatId);
        void UpdateUser(User user);
    }
}
