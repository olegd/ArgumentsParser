namespace ArgumentParser
{
    public interface ICommandMapper
    {
        RoutingData MapCommand(string[] args);
    }
}