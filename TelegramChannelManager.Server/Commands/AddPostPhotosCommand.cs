using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChannelManager.Server.Services;

namespace TelegramChannelManager.Server.Commands
{
    public class AddPostPhotosCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var message = "Добавьте изображения к посту. Отправьте изображения";

            var sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: message,
               cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(sentMessage, UserState.AwaitingNewPostPhotos);
        }
    }
}