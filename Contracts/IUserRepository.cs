using Entities.Models;

namespace Contracts
{
    public interface IUserRepository
    {
        void CreateUser(User user);

        User? GetUser(Guid userId, bool trackChanges);

        IEnumerable<User> GetAllUsers();

        void UpdateUser(User user);

        void DeleteUser(Guid userId);

        void DeleteUser(User user);
    }
}