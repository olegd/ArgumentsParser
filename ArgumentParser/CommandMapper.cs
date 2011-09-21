namespace ArgumentParser
{
    public class CommandMapper : ICommandMapper
    {
        private IHandlersDiscoverer _discoverer;

        public CommandMapper(IHandlersDiscoverer discoverer)
        {
            _discoverer = discoverer;
        }

        public IHandlerDescriptor MapCommand(string[] args)
        {
            //try mapping arguments to known commands.
            //if can't map - fail
            return null;
        }
    }
}