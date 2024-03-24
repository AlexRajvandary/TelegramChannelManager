using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChannelManager.Server.Commands
{
    public interface ICommand
    {
        Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken);
    }
}