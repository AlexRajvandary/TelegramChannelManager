using Shared.DataTransferObjects;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChannelManager.API.Commands.CustomerBotCommands
{
    public class ShowPostCommand : ICommand
    {
        public PostDto? Post { get; set; }

        public Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}