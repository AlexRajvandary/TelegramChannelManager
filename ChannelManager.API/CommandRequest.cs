using System.Text.RegularExpressions;
using ChannelManager.API.Commands;

namespace ChannelManager.API
{
    public class CommandRequest
    {
        private static readonly Regex _commandParser = new Regex(@"^(?<commandName>/[a-zA-Z]+)(:(?<EntityId>[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}))?$");

        private CommandRequest(ICommand command)
        {
            Command = command;
        }

        private CommandRequest(ICommand command, CommandParameter commandParameter)
        {
            Command = command;
            CommandParameter = commandParameter;
        }

        public ICommand Command { get; private set; }

        public CommandParameter? CommandParameter { get; private set; }

        public static bool TryParse(string commandText, 
                                    Dictionary<string, ICommand> commands, 
                                    out CommandRequest? commandRequest)
        {
            var match = _commandParser.Match(commandText);

            if (match.Groups["commandName"].Success)
            {
                if (commands.TryGetValue(match.Groups["commandName"].Value, out var command))
                {
                    if (match.Groups["EntityId"].Success)
                    {
                        var entityId = Guid.Parse(match.Groups["EntityId"].Value);
                        var commandParameter = new CommandParameter(entityId, ParameterType.EntityId);
                        commandRequest = new CommandRequest(command, commandParameter);
                        return true;
                    }
                    else
                    {
                        commandRequest = new CommandRequest(command);
                        return true;
                    }
                }
                else
                {
                    commandRequest = null;
                    return false;
                }
            }
            else
            {
                commandRequest = null;
                return false;
            }
        }
    }
}
