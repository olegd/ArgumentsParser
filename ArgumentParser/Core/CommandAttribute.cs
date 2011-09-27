using System;

namespace ArgumentParser.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        public string CommandName;
    }
}