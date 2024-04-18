using ChannelManager.API.Commands.CustomerBotCommands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChannelManager.API.Commands
{
    public interface ICommand
    {
        Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken);
    }
}