using Telegram.Bot;
using Telegram.Bot.Types;
using Entities.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChannelManager.API.Commands.CustomerBotCommands
{
    public class CreateNewPostCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var message = "Вы можете добавить текст, заголовок и изображения к посту:";

            InlineKeyboardMarkup inlineKeyboard = new(
                   new[]
                   {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Добавить заголовок", "/addposttitle")
                        },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Добавить текст", "/addpostcontent"),
                        },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Добавить изображения", "/addpostphotos"),
                        },

                       new[]
                       {
                           InlineKeyboardButton.WithCallbackData("Сгенерировать идеи", "/generateideas"),
                       },

                       new[]
                       {
                           InlineKeyboardButton.WithCallbackData("Сгенерировать заголовок", "/generatetitle"),
                       },

                        new[]
                       {
                           InlineKeyboardButton.WithCallbackData("Сгенерировать текст", "/generatecontent"),
                       },

                           new []
                        {
                            InlineKeyboardButton.WithCallbackData("Установить время публикации", "/setposttime"),
                        },
                   });

            var sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: message,
               cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(sentMessage, UserState.None);
        }
    }
}