using Telegram.Bot;
using Telegram.Bot.Types;
using ChannelManager.API.Commands;
using ChannelManager.API.Models;

namespace ChannelManager.API.Services
{
    public class UserContext
    {
        private ITelegramBotClient telegramBotClient;

        public UserContext(long chatId)
        {
            ChatId = chatId;
            State = UserState.None;
        }

        public long ChatId { get; }

        public UserState State { get; private set; }

        public List<Post> Posts { get; set; }

        public Post CurrentPost { get; set; }

        public async Task AddNewPost(Post post)
        {
            Posts.Add(post);
        }

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

        public async Task<Message?> ExecuteCommand(ITelegramBotClient telegramBotClient, ICommand command, CancellationToken cancellationToken)
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