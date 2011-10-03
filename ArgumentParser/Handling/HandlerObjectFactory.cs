using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArgumentParser.Handling
{
    public class HandlerObjectFactory
    {
        public static object Create(MethodInfo handlerMethod)
        {
            if (handlerMethod.IsStatic)
            {
                return null;
            }
            
            return ConstructAnObjectForMethod(handlerMethod);
        }

        private static object ConstructAnObjectForMethod(MethodInfo handlerMethod)
        {
            Type handlerDeclaringType = handlerMethod.DeclaringType;
            var constructors = handlerDeclaringType.GetConstructors();

            var resolvableConstructors = constructors.Select(TryResolveConstructorArguments);
            var electedConstructor = ElectConstructor(resolvableConstructors);
            return electedConstructor.Invoke();
        }
        
        private static ResolvableConstructorInfo TryResolveConstructorArguments(ConstructorInfo constructorInfo)
        {
            var ctorParams = constructorInfo.GetParameters();
            var ctorParamValues = new List<object>();
            foreach (var ctorParam in ctorParams)
            {
                object ctorParamValue;
                if (TryGetFromServiceLocator(ctorParam.ParameterType, out ctorParamValue))
                {
                    ctorParamValues.Add(ctorParamValue);
                }
                else
                {
                    return null;
                }
            }
            return new ResolvableConstructorInfo(constructorInfo, ctorParamValues);
        }

        private static bool TryGetFromServiceLocator(Type type, out object value)
        {
            value = null;
            try
            {
                value = DependencyResolver.Current.GetService(type);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static ResolvableConstructorInfo ElectConstructor(IEnumerable<ResolvableConstructorInfo> resolvableConstructors)
        {
            var ctorWithMaxNumberOfArguments =
                resolvableConstructors.Aggregate(
                    (agg, next) => next.ParameterValues.Count > agg.ParameterValues.Count ? next : agg);

            return ctorWithMaxNumberOfArguments;
        }

        private class ResolvableConstructorInfo
        {
            public ConstructorInfo ConstructorInfo { get; private set; }
            public List<object> ParameterValues { get; private set; }

            public ResolvableConstructorInfo(ConstructorInfo constructorInfo, List<object> parameterValues)
            {
                ConstructorInfo = constructorInfo;
                ParameterValues = parameterValues;
            }

            public object Invoke()
            {
                return ConstructorInfo.Invoke(ParameterValues.ToArray());
            }
        }
    }
}