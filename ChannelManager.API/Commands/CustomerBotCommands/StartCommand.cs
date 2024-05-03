using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace ChannelManager.API.Commands.CustomerBotCommands
{
    public class StartCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var text = "Добро пожаловать в #1 Telegram Bot для копирования и агрегации сообщений! " +
                       "Копирую сообщения из каналов, групп и чатов или собираю их в одну ленту. Я умею все, в том числе и искусственный интеллект!";
            InlineKeyboardMarkup inlineKeyboard = new(
                    new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Создать новый пост", "/createnewpost")
                        },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Сохранённые посты", "/showallposts"),
                        }
                    });

            var message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(message, Entities.Models.UserState.None);
        }
    }
}