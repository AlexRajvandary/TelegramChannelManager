using Entities.Models;

namespace Contracts
{
    public interface IUserRepository
    {
        void CreateUser(User user);

        User? GetUser(Guid userId, bool trackChanges);

        User? GetUserByMainChatId(long mainChatId, bool trackChanges);

        User? GetUserByPersonalChatId(long customerChatId, bool trackChanges);

        IEnumerable<User> GetAllUsers();

        void UpdateUser(User user);

        void DeleteUser(Guid userId);

        void DeleteUser(User user);
    }
}