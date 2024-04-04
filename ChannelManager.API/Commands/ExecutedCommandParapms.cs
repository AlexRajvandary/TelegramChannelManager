using Telegram.Bot.Types;
using Entities.Models;

namespace ChannelManager.API.Commands
{
    public class ExecutedCommandParapms
    {
        public ExecutedCommandParapms(Message sentMessage) : this(sentMessage, null)
        {
            SentMessage = sentMessage;
        }

        public ExecutedCommandParapms(Message sentMessage, UserState? userState)
        {
            SentMessage = sentMessage;
            UserState = userState;
        }

        public Message SentMessage { get; }

        public UserState? UserState { get; }
    }
}