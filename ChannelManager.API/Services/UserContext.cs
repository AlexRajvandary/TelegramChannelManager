using Telegram.Bot;
using Telegram.Bot.Types;
using ChannelManager.API.Commands;
using Entities.Models;

namespace ChannelManager.API.Services
{
    public class UserContext
    {
        private ITelegramBotClient? telegramBotClient;

        public UserContext(long chatId)
        {
            ChatId = chatId;
            State = UserState.None;
        }

        public UserContext(long chatId, UserState userState)
        {
            ChatId = chatId;
            State = userState;
        }

        public Guid UserId { get; }

        public long ChatId { get; }

        public UserState State { get; private set; }

        public Post? LastEditedPost { get; set; }

        public async Task CreateTelegramClientAsync(string token, string webhookAdress)
        {
            telegramBotClient = new TelegramBotClient(token);
            await telegramBotClient.SetWebhookAsync(webhookAdress);
            State = UserState.None;
        }

        public async Task<Message?> ExecuteCommand(ICommand command, CancellationToken cancellationToken)
        {
            return await ExecuteCommand(telegramBotClient, command, cancellationToken);
        }

        public async Task<Message?> ExecuteCommand(ITelegramBotClient? telegramBotClient, ICommand command, CancellationToken cancellationToken)
        {
            if (telegramBotClient == null)
            {
                return null;
            }

            var sentMessageParams = await command.ExecuteAsync(telegramBotClient, ChatId, cancellationToken);

            if (sentMessageParams is not null && sentMessageParams.UserState.HasValue)
            {
                State = sentMessageParams.UserState.Value;
            }
           
            return sentMessageParams?.SentMessage;
        }
    }
}