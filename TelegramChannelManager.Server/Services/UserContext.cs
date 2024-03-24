using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChannelManager.Server.Commands;

namespace TelegramChannelManager.Server.Services
{
    public class UserContext
    {
        public UserContext(long chatId)
        {
            ChatId = chatId;
            State = UserState.None;
        }

        public ITelegramBotClient TelegramBotClient { get; private set; }

        public long ChatId { get; }

        public UserState State { get; private set; }

        public async Task CreateTelegramClientAsync(string token, string webhookAdress)
        {
            TelegramBotClient = new TelegramBotClient(token);
            await TelegramBotClient.SetWebhookAsync(webhookAdress);
            State = UserState.None;
        }

        public async Task<Message?> ExecuteCommand(ICommand command, CancellationToken cancellationToken)
        {
            return await ExecuteCommand(TelegramBotClient, command, cancellationToken);
        }

        public async Task<Message?> ExecuteCommand(ITelegramBotClient telegramBotClient, ICommand command, CancellationToken cancellationToken)
        {
            if (telegramBotClient == null)
            {
                return null;
            }

            var sentMessageParams = await command.ExecuteAsync(TelegramBotClient, ChatId, cancellationToken);

            if (sentMessageParams is not null && sentMessageParams.UserState.HasValue)
            {
                State = sentMessageParams.UserState.Value;
            }
           
            return sentMessageParams?.SentMessage;
        }
    }
}