using Entities.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ChannelManager.API.Commands
{
    public class BotWasSuccessfullyCreatedCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var message = "Поздравляем! Ваш персональный бот был добавлен и теперь вы можете пользоваться всеми функциями нашего сервиса через ваш персональный бот.";
            
            var sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: message,
               cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(sentMessage, UserState.None);
        }
    }
}