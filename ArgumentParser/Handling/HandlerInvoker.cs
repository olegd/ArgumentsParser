using System;
using System.Collections.Generic;
using System.Linq;
using ArgumentParser.Core;
using ArgumentParser.Routing;

namespace ArgumentParser.Handling
{
    public class HandlerInvoker : IHandlerInvoker
    {
        public void Invoke(IHandler handler, string[] args)
        {
            //only support for static methods for now..
            if (!handler.HandlerMethodInfo.IsStatic)
            {
                throw new NotSupportedException("Only static methods can be used as handlers");
            }

            var argumentValues = MapArguments(handler, args);
            object[] parameters = GetParameterValues(handler, argumentValues);
            handler.HandlerMethodInfo.Invoke(null, parameters);
        }

        public Dictionary<string, object> MapArguments(IHandler handler, string[] args)
        {
            var argumentValues = new Dictionary<string, object>();

            var unmappedArguemtns = new Queue<string>(handler.SupportedArguments);
            int i = 1;
            while (i < args.Length)
            {
                string nextArg = args[i];
                if (IsAFlag(handler, nextArg))
                {
                    argumentValues[nextArg] = true;
                }
                else //must be an argument then
                {
                    if (unmappedArguemtns.Any() == false) //don't have anything else to map
                    {
                        throw new CommandMappingException(
                            "Number of arguments exceeded the number of argument type parameters on handler method");
                    }
                    var argumentCandidate = unmappedArguemtns.Dequeue();
                    argumentValues[argumentCandidate] = nextArg;
                }
                i++;
            }

            //have something that wasn't mapped
            if (unmappedArguemtns.Any())
            {
                throw new CommandMappingException(
                    "Some of the arguments could not be bound to any paramenter on handler method");
            }

            return argumentValues;
        }
        
        private bool IsAFlag(IHandler handler, string nextArg)
        {
            return handler.SupportedFlags.Contains(nextArg);
        }

        private object[] GetParameterValues(IHandler handler, Dictionary<string, object> argumentValues)
        {
            var handlerParametersInfo = handler.HandlerMethodInfo.GetParameters();

            var result = new List<object>();
            foreach (var parameterInfo in handlerParametersInfo)
            {
                if (argumentValues.ContainsKey(parameterInfo.Name))
                {
                    result.Add(argumentValues[parameterInfo.Name]);
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
