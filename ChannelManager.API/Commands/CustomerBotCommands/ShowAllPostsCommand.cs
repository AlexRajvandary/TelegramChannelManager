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

            var keyboard = new List<List<InlineKeyboardButton>>();

            for(var i = 0; i < Posts.Count; i++)
            {
                if(i == 101)
                {
                    break;
                }

                keyboard.Add([InlineKeyboardButton.WithCallbackData(Posts[i].Title, $"/showpost:{Posts[i].Id}")]);
            }

            sentMessage = await botClient.SendTextMessageAsync(chatId, "Ваши посты:");
            return new ExecutedCommandParapms(sentMessage, UserState.None);
        }
    }
}