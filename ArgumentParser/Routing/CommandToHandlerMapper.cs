using System.Linq;
using ArgumentParser.Configuration;
using ArgumentParser.Core;

namespace ArgumentParser.Routing
{
    public class CommandToHandlerMapper : ICommandToHandlerMapper
    {
        private readonly IHandlerProvider _handlerProvider;

        public CommandToHandlerMapper(IHandlerProvider handlerProvider)
        {
            _handlerProvider = handlerProvider;
        }

        public IHandler Map(string[] args)
        {
            IHandler handler = FindCommandHandler(args);
            if(handler.CanMapArguments(args))
            {
                return handler;
            }
            return null;
        }

        private IHandler FindCommandHandler(string[] args)
        {
            var handlers = _handlerProvider.GetHandlers();
            var handler = handlers.Where(x => x.CanHandleCommand(args)).FirstOrDefault();

            if (handler == null)
            {
                throw new CommandMappingException("Handler for command: {0} does not exist".With(args[0]));
            }
            return handler;
        }
    }
}