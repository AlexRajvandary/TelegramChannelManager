using Contracts;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using Telegram.Bot;

namespace ChannelManager.API.Services
{
    public class TelegramClientsManager : ITelegramClientsManager
    {
        private ConcurrentDictionary<Guid, ITelegramBotClient> telegramBotClients;
        private BotConfiguration _botConfig;
        private ILoggerManager _logger;

        public TelegramClientsManager(IOptions<BotConfiguration> botOptions, ILoggerManager logger)
        {
            telegramBotClients = new ConcurrentDictionary<Guid, ITelegramBotClient>();
            _botConfig = botOptions.Value;
            _logger = logger;
        }

        public async Task<ITelegramBotClient> TryGetOrCreateNewBotClientAsync(Guid userId, string botToken, CancellationToken cancellationToken)
        {
            if (telegramBotClients.TryGetValue(userId, out ITelegramBotClient? value))
            {
                return value;
            }
            else
            {
                var userBotClient = new TelegramBotClient(botToken);
                var webhookAddress = $"{_botConfig.HostAddress}/customerBot";
                _logger.LogInfo($"Setting webhook: {webhookAddress}");
                await userBotClient.SetWebhookAsync(
                    url: webhookAddress,
                    cancellationToken: cancellationToken);

                telegramBotClients.TryAdd(userId, new TelegramBotClient(botToken));
                return userBotClient;
            }
        }

        public ITelegramBotClient? GetBotClient(Guid userId)
        {
            telegramBotClients.TryGetValue(userId, out ITelegramBotClient? value);
            return value;
        }
    }
}
