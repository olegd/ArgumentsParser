using System.Collections.Generic;
using System.Reflection;
using ArgumentParser.Configuration;

namespace ArgumentParser.Routing
{
    public interface IHandler
    {
        string CommandName { get; set; }
        List<string> SupportedArguments { get; set; }
        //keyword value OR keyword:value OR keyword=value
        Dictionary<string, string> SupportedComplexArguments { get; set; }
        MethodInfo HandlerMethodInfo { get; set; }

        Flags Flags { get;  }
        
        

        bool CanHandleCommand(string[] args);
        bool CanMapArguments(string[] args);
        void Invoke(string[] args);
    }
}