using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChannelManager.API.Commands.CustomerBotCommands
{
    public class SavedPostsCommand : ICommand
    {
        public Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}