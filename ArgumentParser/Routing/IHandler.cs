using System.Collections.Generic;
using System.Reflection;

namespace ArgumentParser.Routing
{
    public interface IHandler
    {
        string CommandName { get; set; }
        List<string> SupportedFlags { get; set; }
        List<string> SupportedArguments { get; set; }
        //keyword value OR keyword:value OR keyword=value
        Dictionary<string, string> SupportedComplexArguments { get; set; }
        MethodInfo HandlerMethodInfo { get; set; }
        Dictionary<string, object> ArgumentValues { get; set; }

        bool CanHandleCommand(string[] args);
        bool CanMapArguments(string[] args);
        void Invoke(string[] args);
    }
}