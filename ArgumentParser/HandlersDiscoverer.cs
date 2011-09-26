using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArgumentParser
{
    public class HandlersDiscoverer : IHandlersDiscoverer
    {
        public List<IHandlerDescriptor> GetHandlers()
        {
            IEnumerable<MethodInfo> methodsWithAttr = FindAllMethodsWithCommandAttribute();

            var result = new List<IHandlerDescriptor>();

            foreach (var method in methodsWithAttr)
            {
                var commandDescriptor = new HandlerDescriptor
                                            {
                                                HandlerMethodInfo = method,
                                                CommandName = GetCommandName(method)
                                            };

                var parameters = method.GetParameters();
                
                foreach (var parameter in parameters)
                {
                    if (parameter.ParameterType == typeof(bool))
                    {
                        commandDescriptor.Flags.Add(parameter.Name);
                    }
                    else
                    {
                        commandDescriptor.Arguments.Add(parameter.Name);
                    }
                }

                result.Add(commandDescriptor);
            }

            return result;
        }

        private static IEnumerable<MethodInfo> FindAllMethodsWithCommandAttribute()
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