using Telegram.Bot;

namespace ChannelManager.API.Services
{
    public interface ITelegramClientsManager
    {
        Task<ITelegramBotClient> CreateNewBotClientAsync(Guid userId, string botToken, CancellationToken cancellationToken);

        ITelegramBotClient? GetBotClient(Guid userId);
    }
}
