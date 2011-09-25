using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgumentParser
{
    public class Router
    {
        private ICommandMapper CommandMapper { get; set; }

        public Router()
        {
            //poor man's DI..
            CommandMapper = new CommandMapper(new HandlersDiscoverer());
        }

        public Router(ICommandMapper commandMapper)
        {
            CommandMapper = commandMapper;
        }
        
        public void Route(string[] args)
        {
            var handlerDescriptor = CommandMapper.MapCommand(args);
            
            //only support static methods for now..
            if (!handlerDescriptor.Handler.IsStatic)
            {
                throw new NotSupportedException("Only static methods can be used as handlers");
            }

            object[] parameters = GetParameterValues(handlerDescriptor);
            handlerDescriptor.Handler.Invoke(null, parameters);
        }

        private static object[] GetParameterValues(RoutingData handlerDescriptor)
        {
            var handlerParametersInfo = handlerDescriptor.Handler.GetParameters();

            var result = new List<object>();
            foreach (var parameterInfo in handlerParametersInfo)
            {
                if (handlerDescriptor.ArgumentValues.ContainsKey(parameterInfo.Name))
                {
                    result.Add(handlerDescriptor.ArgumentValues[parameterInfo.Name]);
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