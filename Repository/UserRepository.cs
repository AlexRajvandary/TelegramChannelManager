using Contracts;
using Entities.Models;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateUser(User user) => Create(user);

        public void DeleteUser(Guid userId)
        {
            var user = GetUser(userId, true);

            if(user != null)
            {
                Delete(user);
            }
        }

        public void DeleteUser(User user) => Delete(user);

        public IEnumerable<User> GetAllUsers() => FindAll(false);

        public User? GetUser(Guid userId, bool trackChanges) => FindByCondition(user => user.Id == userId, trackChanges).FirstOrDefault();

        public User? GetUser(long chatId, bool trackChanges) => FindByCondition(user => user.ChatId == chatId, trackChanges).FirstOrDefault();

        public void UpdateUser(User user) => Update(user);
    }
}