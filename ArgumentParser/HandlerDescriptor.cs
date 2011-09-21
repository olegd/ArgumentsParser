using System.Collections.Generic;
using System.Reflection;

namespace ArgumentParser
{
    public class HandlerDescriptor : IHandlerDescriptor
    {
        public string Name { get; set; }
        public List<string> Flags { get; set; }
        public List<string> Arguments { get; set; }
        public Dictionary<string, string> ComplexArguments { get; set; }
        public MethodInfo HandlerMethodInfo { get; set; }


        public HandlerDescriptor()
        {
            Flags = new List<string>();
            Arguments = new List<string>();
            ComplexArguments = new Dictionary<string, string>();
        }
    }
}