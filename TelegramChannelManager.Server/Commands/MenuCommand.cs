using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace TelegramChannelManager.Server.Commands
{
    public class MenuCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new(
                    new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Новый пост", "/newPost")
                        },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Сохранённые посты", "/savedPosts"),
                        }
                    });

            var message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Главное меню",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(message);
        }
    }
}