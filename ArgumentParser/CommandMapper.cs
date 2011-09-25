using System.Collections.Generic;
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
                throw new CommandMappingException("Handler for command: {0} does not exist".With(args[0]));
            }

            var result = new RoutingData();
            result.Handler = handler.HandlerMethodInfo;
            result.HandlerName = handler.Name;

            var unmappedArguemtns = new Queue<string>(handler.Arguments);
            int i = 1;
            while (i < args.Length)
            {
                string nextArg = args[i];
                if (IsAFlag(nextArg, handler))
                {
                    result.ArgumentValues[nextArg] = true.ToString();
                }
                else //must be an argument then
                {
                    if (unmappedArguemtns.Any() == false)//don't have anything else to map
                    {
                        throw new CommandMappingException(
                            "Number of arguments exceeded the number of argument type parameters on handler method");
                    }
                    var argumentCandidate = unmappedArguemtns.Dequeue();
                    result.ArgumentValues[argumentCandidate] = nextArg;
                }
                i++;
            }

            //have something that wasn't mapped
            if (unmappedArguemtns.Any())
            {
                throw new CommandMappingException(
                    "Some of the arguments could not be bound to any paramenter on handler method");
            }

            return result;
        }

        private bool IsAFlag(string nextArg, IHandlerDescriptor handler)
        {
            return handler.Flags.Contains(nextArg);
        }
    }
}