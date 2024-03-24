namespace TelegramChannelManager.Server.Services
{
    public interface IUserContextManager
    {
        UserContext CreateNewUserContext(long chatId);
        UserContext? GetUserContext(long chatId);
        bool TryGetUserContext(long chatId, out UserContext? userContext);
    }
}