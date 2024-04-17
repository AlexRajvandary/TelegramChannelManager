using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace ChannelManager.API.Commands
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
                            InlineKeyboardButton.WithCallbackData("Новый пост", "/newpost")
                        },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Сохранённые посты", "/savedposts"),
                        }
                    });

            var message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Главное меню",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(message, Entities.Models.UserState.None);
        }
    }

    public class SavedPostsCommand : ICommand
    {
        public Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}