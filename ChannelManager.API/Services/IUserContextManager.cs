using Entities.Models;

namespace ChannelManager.API.Services
{
    public interface IUserContextManager
    {
        UserContext CreateNewUserContext(long chatId);
        UserContext? GetUserContext(long chatId);
        bool TryGetUserContext(long chatId, out UserContext? userContext);
        Task<UserContext> RestoreUserContextAsync(User user, string? webhookAdress = null);
    }
}