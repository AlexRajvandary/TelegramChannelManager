using Telegram.Bot.Types;
using TelegramChannelManager.Server.Services;

namespace TelegramChannelManager.Server.Commands
{
    public class ExecutedCommandParapms
    {

        public ExecutedCommandParapms(Message sentMessage) : this(sentMessage, null)
        {

        }

        public ExecutedCommandParapms(UserState userState) : this(null, userState)
        {

        }

        public ExecutedCommandParapms(Message? message, UserState? userState)
        {

        }

        public Message? SentMessage { get; set; }

        public UserState? UserState { get; set; }
    }
}