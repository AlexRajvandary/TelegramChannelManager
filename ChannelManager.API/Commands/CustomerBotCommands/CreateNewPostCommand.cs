using Telegram.Bot;
using Telegram.Bot.Types;
using Entities.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChannelManager.API.Commands.CustomerBotCommands
{
    public class CreateNewPostCommand : ICommand
    {
        public Guid? PostId { get; set; }

        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var message = "Вы можете добавить текст, заголовок и изображения к посту:";

            InlineKeyboardMarkup inlineKeyboard = new(
                   new[]
                   {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Добавить заголовок", $"/addposttitle:{PostId}")
                        },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Добавить текст", $"/addpostcontent:{PostId}"),
                        },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Добавить изображения", $"/addpostphotos:{PostId}"),
                        },

                       new[]
                       {
                           InlineKeyboardButton.WithCallbackData("Сгенерировать идеи", "/generateideas"),
                       },

                       new[]
                       {
                           InlineKeyboardButton.WithCallbackData("Сгенерировать заголовок", $"/generatetitle:{PostId}"),
                       },

                       new[]
                       {
                           InlineKeyboardButton.WithCallbackData("Сгенерировать текст", $"/generatecontent:{PostId}"),
                       },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Установить время публикации", $"/settime:{PostId}"),
                        },
                   });

            var sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: message,
               replyMarkup: inlineKeyboard,
               cancellationToken: cancellationToken);

            PostId = null;

            return new ExecutedCommandParapms(sentMessage, UserState.None);
        }
    }
}