using Telegram.Bot;

namespace ChannelManager.API.Services
{
    public interface ITelegramClientsManager
    {
        Task<ITelegramBotClient> TryGetOrCreateNewBotClientAsync(Guid userId, string botToken, CancellationToken cancellationToken);

        ITelegramBotClient? GetBotClient(Guid userId);
    }
}
