namespace ChannelManager.API
{
    public class CommandParameter
    {
        public CommandParameter(Guid parameter, ParameterType type) 
        {
            Parameter = parameter;
            Type = type;
        }

        public Guid Parameter { get; set; }

        public ParameterType Type { get; set; }
    }
}
