using ChannelManager.API.Commands;

namespace ChannelManager.API.Extensions
{
    public static class CommandExtensions
    {
        public static string GetCommandName(this ICommand command)
        {
            return "/" + command.GetType().Name.Replace("Command", "", StringComparison.OrdinalIgnoreCase).ToLowerInvariant();
        }

        public static string GetCommandName(this Type commandType)
        {
            return "/" + commandType.Name.Replace("Command", "", StringComparison.OrdinalIgnoreCase).ToLowerInvariant();
        }
    }
}