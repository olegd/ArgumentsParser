namespace ArgumentParser.Routing
{
    public interface ICommandToHandlerMapper
    {
        IHandler Map(string[] args);
    }
}