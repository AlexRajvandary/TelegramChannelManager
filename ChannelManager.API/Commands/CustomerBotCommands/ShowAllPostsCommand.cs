using Entities.Models;
using Shared.DataTransferObjects;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChannelManager.API.Commands.CustomerBotCommands
{
    public class ShowAllPostsCommand : ICommand
    {
        public List<PostDto>? Posts { get; set; }

        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            Message? sentMessage = null;
            if(Posts is null || Posts.Count == 0)
            {
                sentMessage = await botClient.SendTextMessageAsync(chatId, "Постов нет", cancellationToken: cancellationToken);
                return new ExecutedCommandParapms(sentMessage, UserState.None);
            }

            var keyboard = new InlineKeyboardButton[Posts.Count][];
            for (var i = 0; i < Posts.Count; i++)
            {
                if (i == 101)
                {
                    break;
                }

                keyboard[i] = new InlineKeyboardButton[] { InlineKeyboardButton.WithCallbackData(Posts[i].Title, $"/showpost:{Posts[i].Id}") };
            }

            var inlineKeyboard = new InlineKeyboardMarkup(keyboard);

            sentMessage = await botClient.SendTextMessageAsync(chatId, "Ваши посты:", replyMarkup: inlineKeyboard);
            return new ExecutedCommandParapms(sentMessage, UserState.None);
        }
    }
}