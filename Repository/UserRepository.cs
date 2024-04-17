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

        public User? GetUser(Guid userId, bool trackChanges) => FindByCondition(user => user.Id.Equals(userId), trackChanges).SingleOrDefault();

        public User? GetUserByMainChatId(long chatId, bool trackChanges) => FindByCondition(user => user.MainChatId.Equals(chatId), trackChanges).SingleOrDefault();

        public User? GetUserByPersonalChatId(long personalChatId, bool trackChanges) => FindByCondition(user => user.PersonalChatId.Equals(personalChatId), trackChanges).SingleOrDefault();

        public void UpdateUser(User user) => Update(user);
    }
}