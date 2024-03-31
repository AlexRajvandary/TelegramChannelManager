using Telegram.Bot;
using Telegram.Bot.Types;
using ChannelManager.API.Services;

namespace ChannelManager.API.Commands
{
    public class IncorrectTokenCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var askApiTokenMessage = "Некорректный токен. Введите API-токен повторно:";

            var sentMessage = await botClient.SendTextMessageAsync(chatId: chatId,
                                                                   text: askApiTokenMessage,
                                                                   cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(sentMessage, UserState.AwaitingToken);
        }
    }
}