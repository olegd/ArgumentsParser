using ArgumentParser.Configuration;
using ArgumentParser.Routing;

namespace ArgumentParser.Handling
{
    public class Router
    {
        private ICommandToHandlerMapper CommandToHandlerMapper { get; set; }

        public Router()
        {
            //poor man's DI..
            CommandToHandlerMapper = new CommandToHandlerMapper(new HandlerProvider());
        }

        public Router(ICommandToHandlerMapper commandToHandlerMapper)
        {
            CommandToHandlerMapper = commandToHandlerMapper;
        }
        
        public void Route(string[] args)
        {
            var handler = CommandToHandlerMapper.Map(args);
            handler.Invoke(args);
        }
    }
}