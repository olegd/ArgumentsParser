using System;

namespace ArgumentParser.Core
{
    public class CommandMappingException : Exception
    {
        public CommandMappingException()
        {
        }

        public CommandMappingException(string message) : base(message)
        {
        }
    }
}