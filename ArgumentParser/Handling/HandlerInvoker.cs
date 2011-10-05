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
            var argumentValues = MapArguments(handler, args);
            object[] parameters = GetParameterValues(handler, argumentValues);

            InvokeHandler(handler, parameters);
        }

        private void InvokeHandler(IHandler handler, object[] parameters)
        {
            var handlerObject = HandlerObjectFactory.Create(handler.HandlerMethodInfo);
            handler.HandlerMethodInfo.Invoke(handlerObject, parameters);
        }

        public Dictionary<string, object> MapArguments(IHandler handler, string[] args)
        {
            var trimmedArgs = args.RemoveEmptyElements();

            var argumentValues = new Dictionary<string, object>();

            var unmappedArguemtns = new Queue<string>(handler.SupportedArguments);
            int i = 1;
            while (i < trimmedArgs.Length)
            {
                string nextArg = trimmedArgs[i];
                if (handler.Flags.Resolve(nextArg) != null)
                {
                    var resolvedFlag = handler.Flags.Resolve(nextArg);
                    argumentValues[resolvedFlag] = true;
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

        private static object[] GetParameterValues(IHandler handler, Dictionary<string, object> argumentValues)
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
