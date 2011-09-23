using System.Linq;

namespace ArgumentParser
{
    public class CommandMapper : ICommandMapper
    {
        private IHandlersDiscoverer _discoverer;

        public CommandMapper(IHandlersDiscoverer discoverer)
        {
            _discoverer = discoverer;
        }

        public RoutingData MapCommand(string[] args)
        {
            var handlers = _discoverer.GetHandlers();
            var handler = handlers.Where(x => x.Name == args[0]).FirstOrDefault();

            if (handler == null)
            {
                return null;
            }

            var result = new RoutingData();
            result.Handler = handler.HandlerMethodInfo;
            result.HandlerName = handler.Name;

            int mappedArgumentCount = 0;
            int i = 1;
            while (i < args.Length)
            {
                string nextArg = args[i];
                if (IsAFlag(nextArg, handler))
                {
                    result.ArgumentValues[nextArg] = true.ToString();
                }
                else // must be an argument
                {
                    result.ArgumentValues[handler.Arguments[0]] = nextArg; 
                }

                
                i++;
            }


            //if next arg matches a flag, it's a flag
            //if it matches complex argument name, read next arg to get the value
            //

            //try mapping arguments to known commands.
            //if can't map - fail
            return result;
        }

        private bool IsAFlag(string nextArg, IHandlerDescriptor handler)
        {
            return handler.Flags.Contains(nextArg);
        }
    }
}