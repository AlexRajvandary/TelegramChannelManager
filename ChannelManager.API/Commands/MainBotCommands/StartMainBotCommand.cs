using Entities.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChannelManager.API.Commands.MainBotCommands
{
    public class StartMainBotCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var message = "Для подключения Telegram, Вам необходимо: создать бота через @BotFather, после создания Вам будет выдан токен бота - ключ доступа. Добавьте своего бота в канал или группу и подключите аккаунт";
            var askApiTokenMessage = "Введите API-токен:";

            InlineKeyboardMarkup inlineKeyboard = new(
                    new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Прочитать инструкцию по подключению Telegram", "/manual")
                        },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Спросить в чате", "/askSupport"),
                        }
                    });

            await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: message,
               replyMarkup: inlineKeyboard,
               cancellationToken: cancellationToken);

            var sentMessage = await botClient.SendTextMessageAsync(chatId: chatId,
                                                                   text: askApiTokenMessage,
                                                                   cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(sentMessage, UserState.AwaitingToken);
        }
    }
}