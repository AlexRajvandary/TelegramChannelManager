using Entities.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChannelManager.API.Commands.CustomerBotCommands
{
    public class AddPostReactionsCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var message = "Добавьте реакции к посту. Для этого просто отправьте эмодзи, которые вы хотите добавить в качестве реакций";

            var sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: message,
               cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(sentMessage, UserState.AwaitingNewPostReactions);
        }
    }
}