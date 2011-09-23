using System.Collections.Generic;
using System.Reflection;

namespace ArgumentParser
{
    public interface IHandlerDescriptor
    {
        string Name { get; set; }
        List<string> Flags { get; set; }
        List<string> Arguments { get; set; }
        Dictionary<string, string> ComplexArguments { get; set; }//keyword value OR keyword:value OR keyword=value
        MethodInfo HandlerMethodInfo { get; set; }
    }
}