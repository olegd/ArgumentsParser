using System.Collections.Generic;
using System.Reflection;

namespace ArgumentParser
{
    public class RoutingData
    {
        public MethodInfo Handler { get; set; }
        public string HandlerName { get; set; }
        public Dictionary<string, string> ArgumentValues { get; set; }

        public RoutingData()
        {
            ArgumentValues = new Dictionary<string, string>();
        }
    }
}