using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace TelegramChannelManager.Server.Commands
{
    public class MenuCommand : ICommand
    {
        public static string Name => "/menu";

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

    public class UsageCommand : ICommand
    {
        public static string Name => "/usage";

        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var text = "Добро пожаловать в #1 Telegram Bot для копирования и агрегации сообщений! " +
                       "Копирую сообщения из каналов, групп и чатов или собираю их в одну ленту. Я умею все, в том числе и искусственный интеллект!";

            var message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(message);
        }
    }
}