using ArgumentParser.Routing;

namespace ArgumentParser.Tests
{
    public class HandlerObjectMother
    {
        public static IHandler CreateHandler(string name, string[] arguments = null, string[] flags = null)
        {
            arguments = arguments ?? new string[0];
            flags = flags ?? new string[0];

            var handler = new Handler { CommandName = name };
            handler.SupportedArguments.AddRange(arguments);
            handler.SupportedFlags.AddRange(flags);

            return handler;
        } 
    }
}