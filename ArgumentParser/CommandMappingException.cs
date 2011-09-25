using System;

namespace ArgumentParser
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