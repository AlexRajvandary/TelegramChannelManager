﻿using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChannelManager.Server.Services;

namespace TelegramChannelManager.Server.Commands
{
    public class AddPostContentCommand : ICommand
    {
        public async Task<ExecutedCommandParapms> ExecuteAsync(ITelegramBotClient botClient, ChatId chatId, CancellationToken cancellationToken)
        {
            var message = "Введите текст поста:";

            var sentMessage = await botClient.SendTextMessageAsync(
               chatId: chatId,
               text: message,
               cancellationToken: cancellationToken);

            return new ExecutedCommandParapms(sentMessage, UserState.AwaitingNewPostContent);
        }
    }
}