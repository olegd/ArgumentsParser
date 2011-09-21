namespace ArgumentParser
{
    public interface ICommandMapper
    {
        IHandlerDescriptor MapCommand(string[] args);
    }
}