using Telegram.Bot;

namespace ChannelManager.API.Services
{
    public class UserContextManager : IUserContextManager
    {
        private Dictionary<long, UserContext> userContexts;

        public UserContextManager()
        {
            userContexts = new Dictionary<long, UserContext>();
        }

        public UserContext CreateNewUserContext(long chatId)
        {
            var userContext = new UserContext(chatId);
            userContexts.TryAdd(chatId, userContext);
            return userContext;
        }

        public UserContext? GetUserContext(long chatId)
        {
            if (userContexts == null)
            {
                return null;
            }
            else if (userContexts.TryGetValue(chatId, out var userContext))
            {
                return userContext;
            }
            else
            {
                return null;
            }
        }

        public bool TryGetUserContext(long chatId, out UserContext? userContext)
        {
            userContext = GetUserContext(chatId);

            if (userContext == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Dictionary<long, UserContext> RestoreUserContexts()
        {
            return new Dictionary<long, UserContext>();
        }
    }
}