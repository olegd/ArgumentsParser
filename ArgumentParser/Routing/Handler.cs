using System;
using System.Collections.Generic;
using System.Reflection;
using ArgumentParser.Configuration;
using ArgumentParser.Core;
using ArgumentParser.Handling;

namespace ArgumentParser.Routing
{
    public class Handler : IHandler
    {
        public string CommandName { get; set; }
        public List<string> SupportedArguments { get; set; }
        public Dictionary<string, string> SupportedComplexArguments { get; set; }
        public MethodInfo HandlerMethodInfo { get; set; }

        public Flags Flags { get; private set; }

        public IHandlerInvoker HandlerInvoker { get; set; }

        public Handler()
        {
            SupportedArguments = new List<string>();
            SupportedComplexArguments = new Dictionary<string, string>();
            HandlerInvoker = new HandlerInvoker();
            Flags = new Flags();
        }

        public bool CanHandleCommand(string[] args)
        {
            var commandString = args[0].Trim();
            return String.Equals(commandString, CommandName);
        }

        public bool CanMapArguments(string[] args)
        {
            try
            {
                HandlerInvoker.MapArguments(this, args);
            }
            catch (CommandMappingException)//@refactor: is there a better way?
            {
                return false;
            }
            return true;
        }
        
        public void Invoke(string[] args)
        {
            HandlerInvoker.Invoke(this, args);
        }
    }
}