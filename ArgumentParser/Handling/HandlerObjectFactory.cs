// *******************************************************************************
// * Copyright (c) 1999 - 2011.
// * Global Relay Communications Inc.
// * All rights reserved.
// *******************************************************************************

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;
using System.Linq;

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
            
            return ConstructAnObject(handlerMethod);
        }

        private static object ConstructAnObject(MethodInfo handlerMethod)
        {
            Type declaringType = handlerMethod.DeclaringType;
            var constructors = declaringType.GetConstructors();

            var resolvableConstructors = new Dictionary<ConstructorInfo, List<object>>();


            foreach (var constructorInfo in constructors)
            {
                var ctorParams = constructorInfo.GetParameters();
                bool couldResolveAllParams = true;
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
                        couldResolveAllParams = false;
                        break;
                    }
                }

                if (couldResolveAllParams == false)
                {
                    continue;
                }


                resolvableConstructors.Add(constructorInfo, ctorParamValues);
            }

            //find ctor with max arg
            var ctorWithMaxArguments = resolvableConstructors.Aggregate((agg, next) => next.Value.Count > agg.Value.Count ? next : agg);
            var handlerObject = ctorWithMaxArguments.Key.Invoke(ctorWithMaxArguments.Value.ToArray());

            return handlerObject;
        }

        private static bool TryGetFromServiceLocator(Type type, out object value)
        {
            value = null;
            try
            {
                value = ServiceLocator.Current.GetInstance(type);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}