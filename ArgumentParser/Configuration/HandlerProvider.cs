using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArgumentParser.Core;
using ArgumentParser.Routing;

namespace ArgumentParser.Configuration
{
    public class HandlerProvider : IHandlerProvider
    {
        public List<IHandler> GetHandlers()
        {
            IEnumerable<MethodInfo> methodsWithAttr = FindAllMethodsWithCommandAttribute();

            var result = new List<IHandler>();

            foreach (var method in methodsWithAttr)
            {
                var commandDescriptor = new Handler
                                            {
                                                HandlerMethodInfo = method,
                                                CommandName = GetCommandName(method)
                                            };

                var parameters = method.GetParameters();
                
                foreach (var parameter in parameters)
                {
                    if (parameter.ParameterType == typeof(bool))
                    {
                        commandDescriptor.SupportedFlags.Add(parameter.Name);
                    }
                    else
                    {
                        commandDescriptor.SupportedArguments.Add(parameter.Name);
                    }
                }

                result.Add(commandDescriptor);
            }

            return result;
        }

        private IEnumerable<MethodInfo> FindAllMethodsWithCommandAttribute()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var methodsWithAttr = from a in assemblies
                                  from type in a.GetTypes()
                                  from member in type.GetMethods()
                                  where Attribute.IsDefined(member, typeof (CommandAttribute))
                                  select member;
            return methodsWithAttr;
        }

        private string GetCommandName(MethodInfo handlerInfo)
        {
            var customAttribute = (CommandAttribute)Attribute.GetCustomAttribute(handlerInfo, typeof(CommandAttribute));
            return string.IsNullOrWhiteSpace(customAttribute.CommandName)
                                                ? handlerInfo.Name
                                                : customAttribute.CommandName;
        }
    }
}