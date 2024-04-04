using Entities.Models;
using System.Collections.Concurrent;

namespace ChannelManager.API.Services
{
    public class UserContextManager : IUserContextManager
    {
        private ConcurrentDictionary<long, UserContext> _userContexts;

        public UserContextManager()
        {
            _userContexts = new ConcurrentDictionary<long, UserContext>();
        }

        public UserContext CreateNewUserContext(long chatId)
        {
            var userContext = new UserContext(chatId);
            _userContexts.TryAdd(chatId, userContext);
            return userContext;
        }

        public async Task<UserContext> RestoreUserContextAsync(User user, string? webhookAdress = null)
        {
            var userContext = new UserContext(user.ChatId, user.State);

            if (!string.IsNullOrWhiteSpace(user.BotToken) && !string.IsNullOrWhiteSpace(webhookAdress))
            {
                await userContext.CreateTelegramClientAsync(user.BotToken, webhookAdress);
            }

            _userContexts.TryAdd(user.ChatId, userContext);
            return userContext;
        }

        public UserContext? GetUserContext(long chatId)
        {
            if (_userContexts == null)
            {
                return null;
            }
            else if (_userContexts.TryGetValue(chatId, out var userContext))
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
            return userContext != null;
        }
    }
}