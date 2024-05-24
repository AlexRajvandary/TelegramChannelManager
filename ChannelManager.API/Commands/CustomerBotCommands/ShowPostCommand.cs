using Entities.Models;
using Shared.DataTransferObjects;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChannelManager.API.Commands.CustomerBotCommands
{
    public class ShowPostCommand : ICommand
    {
        public PostDto? Post { get; set; }

        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var text = $"{Post.Title}\n\n{Post.Content}";

            InlineKeyboardMarkup inlineKeyboard = new(
                 new[]
                 {
                     new []
                     {
                            InlineKeyboardButton.WithCallbackData("Изменить заголовок", $"/addposttitle:{Post.Id}")
                        },
                     new []
                     {
                            InlineKeyboardButton.WithCallbackData("Изменить текст", $"/addpostcontent:{Post.Id}"),
                        },
                     new []
                     {
                            InlineKeyboardButton.WithCallbackData("Изменить изображения", $"/addpostphotos:{Post.Id}"),
                        },

                       new[]
                       {
                           InlineKeyboardButton.WithCallbackData("Сгенерировать идеи", "/generateideas"),
                       },
                     new[]
                     {
                           InlineKeyboardButton.WithCallbackData("Сгенерировать заголовок", $"/generatetitle:{Post.Id}"),
                       },
                     new[]
                     {
                           InlineKeyboardButton.WithCallbackData("Сгенерировать текст", $"/generatecontent:{Post.Id}"),
                       },

                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Установить время публикации", $"/settime:{Post.Id}"),
                        },
                 });

            var sentMessage = await botClient.SendTextMessageAsync(chatId, text, replyMarkup: inlineKeyboard, cancellationToken: cancellationToken);
            return new ExecutedCommandParapms(sentMessage, Entities.Models.UserState.None);
        }
    }
}