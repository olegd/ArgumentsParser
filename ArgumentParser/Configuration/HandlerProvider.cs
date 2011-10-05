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

                var synonymsMap = GetsynonymsMap(commandDescriptor, method);

                foreach (var parameter in parameters)
                {
                    if (parameter.ParameterType == typeof(bool))
                    {
//                        commandDescriptor.SupportedFlags.Add(parameter.Name);
                        string synonyms = null; 
                        if (synonymsMap.ContainsKey(parameter.Name))
                        {
                            synonyms = synonymsMap[parameter.Name];
                        }
                        commandDescriptor.Flags.Add(parameter.Name, synonyms);
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

        private static Dictionary<string, string> GetsynonymsMap(Handler commandDescriptor, MethodInfo method)
        {
            var result = new Dictionary<string, string>();
            var synonymAttributes = Attribute.GetCustomAttributes(method, typeof (DefineSynonymAttribute));
            if (synonymAttributes.Any())
            {
                foreach (var synonymAttribute in synonymAttributes)
                {
                    var attribute = ((DefineSynonymAttribute) synonymAttribute);
                    result[attribute.ArgumentName] = attribute.Synonyms;
                }
            }
            return result;
        }

        protected virtual IEnumerable<MethodInfo> FindAllMethodsWithCommandAttribute()
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