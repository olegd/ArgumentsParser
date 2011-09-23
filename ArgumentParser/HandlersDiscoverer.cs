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
            Assembly assembly = Assembly.GetCallingAssembly();

            var methodsWithAttr = from type in assembly.GetTypes()
                                  from member in type.GetMethods()
                                  where Attribute.IsDefined(member, typeof (CommandAttribute))
                                  select member;
            
            var result = new List<IHandlerDescriptor>();

            foreach (var method in methodsWithAttr)
            {
                var commandDescriptor = new HandlerDescriptor
                                            {
                                                Name = method.Name,
                                                HandlerMethodInfo = method
                                            };

                var parameters = method.GetParameters();
                
                foreach (var parameter in parameters)
                {
                    if (parameter.ParameterType == typeof(bool))
                    {
                        commandDescriptor.Flags.Add(parameter.Name);
                    }
                }
            }

            return result;
        }
    }
}