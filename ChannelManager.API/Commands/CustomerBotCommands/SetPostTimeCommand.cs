﻿using Telegram.Bot;
using Telegram.Bot.Types;
using Entities.Models;

namespace ChannelManager.API.Commands.CustomerBotCommands
{
    public class SetPostTimeCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var message = "Введите время и дату публикации поста";

            var sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: message,
               cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(sentMessage, UserState.AwaitingNewPostContent);
        }
    }
}