using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArgumentParser.Core;

namespace ArgumentParser.Routing
{
    public class Handler : IHandler
    {
        public string CommandName { get; set; }
        public List<string> SupportedFlags { get; set; }
        public List<string> SupportedArguments { get; set; }
        public Dictionary<string, string> SupportedComplexArguments { get; set; }
        public MethodInfo HandlerMethodInfo { get; set; }
        public Dictionary<string, object> ArgumentValues { get; set; }

        public Handler()
        {
            SupportedFlags = new List<string>();
            SupportedArguments = new List<string>();
            SupportedComplexArguments = new Dictionary<string, string>();
            ArgumentValues = new Dictionary<string, object>();
        }

        public bool CanHandleCommand(string[] args)
        {
            var commandString = args[0].Trim();
            return String.Equals(commandString, CommandName);
        }


        //@refactor: remove the side effect: argumentValues are populated as part of this method. 
        public bool CanMapArguments(string[] args)
        {
            var unmappedArguemtns = new Queue<string>(SupportedArguments);
            int i = 1;
            while (i < args.Length)
            {
                string nextArg = args[i];
                if (IsAFlag(nextArg))
                {
                    ArgumentValues[nextArg] = true;
                }
                else //must be an argument then
                {
                    if (unmappedArguemtns.Any() == false) //don't have anything else to map
                    {
                        throw new CommandMappingException(
                            "Number of arguments exceeded the number of argument type parameters on handler method");
                    }
                    var argumentCandidate = unmappedArguemtns.Dequeue();
                    ArgumentValues[argumentCandidate] = nextArg;
                }
                i++;
            }

            //have something that wasn't mapped
            if (unmappedArguemtns.Any())
            {
                throw new CommandMappingException(
                    "Some of the arguments could not be bound to any paramenter on handler method");
            }
            //return result;
            return true;
        }

       
        private bool IsAFlag(string nextArg)
        {
            return SupportedFlags.Contains(nextArg);
        }

        public void Invoke(string[] args)
        {
            //only support static methods for now..
            if (!HandlerMethodInfo.IsStatic)
            {
                throw new NotSupportedException("Only static methods can be used as handlers");
            }

            object[] parameters = GetParameterValues();
            HandlerMethodInfo.Invoke(null, parameters);
        }

        private object[] GetParameterValues()
        {
            var handlerParametersInfo = HandlerMethodInfo.GetParameters();

            var result = new List<object>();
            foreach (var parameterInfo in handlerParametersInfo)
            {
                if (ArgumentValues.ContainsKey(parameterInfo.Name))
                {
                    result.Add(ArgumentValues[parameterInfo.Name]);
                }
                else
                {
                    //if parameter is a flag, and we don't have a value for it
                    if (parameterInfo.ParameterType == typeof(bool))
                    {
                        result.Add(false);//default all flags to false
                    }
                }
            }

            return Enumerable.ToArray(result);
        }
    }
}