namespace ArgumentParser
{
    public class Router
    {
        private ICommandMapper CommandMapper { get; set; }

        public Router()
        {
        }

        public Router(ICommandMapper commandMapper)
        {
            CommandMapper = commandMapper;
        }
        
        public void Route(string[] args)
        {
            var handlerDescriptor = CommandMapper.MapCommand(args);
            //execute command here..
        }
    }
}