using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using ArgumentParser.Configuration;
using NUnit.Framework;
using System.Linq;

namespace ArgumentParser.Tests.Configuration
{
    [TestFixture]
    public class HandlerProviderTests
    {
        [Test]
        public void TESTNAME()
        {
//            var methodInfo = new MethodInfoFake
//                                 {
//                                     GetParametersResult = new List<ParameterInfo>
//                                                               {
//                                                                   new ParameterInfoFake
//                                                                       {
//
//                                                                       }
//                                                               }
//                                 };
//            var handlerProvider = CreateHandlerProvider();
//
//            var result = handlerProvider.GetHandlers();
        }

        private HandlerProviderForTest CreateHandlerProvider(MethodInfoFake findAllMethodsWithCommandAttributeResult = null)
        {
            var returnedMethodInfo = findAllMethodsWithCommandAttributeResult ?? new MethodInfoFake();
            return new HandlerProviderForTest(
                new List<MethodInfo> { returnedMethodInfo }
            );
        }


        private class HandlerProviderForTest : HandlerProvider
        {
            public IEnumerable<MethodInfo> FindAllMethodsWithCommandAttributeResult { get; set; }

            public HandlerProviderForTest(IEnumerable<MethodInfo> findAllMethodsWithCommandAttributeResult)
            {
                FindAllMethodsWithCommandAttributeResult = findAllMethodsWithCommandAttributeResult;
            }

            protected override IEnumerable<MethodInfo> FindAllMethodsWithCommandAttribute()
            {
                return FindAllMethodsWithCommandAttributeResult;
            }
        }

        private class MethodInfoFake : MethodInfo
        {
            public override object[] GetCustomAttributes(bool inherit)
            {
                throw new NotImplementedException();
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public List<ParameterInfo> GetParametersResult { get; set; }

            public override ParameterInfo[] GetParameters()
            {
                return Enumerable.ToArray(GetParametersResult);
            }

            public override MethodImplAttributes GetMethodImplementationFlags()
            {
                throw new NotImplementedException();
            }

            public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
            {
                throw new NotImplementedException();
            }

            public override MethodInfo GetBaseDefinition()
            {
                throw new NotImplementedException();
            }

            public override ICustomAttributeProvider ReturnTypeCustomAttributes
            {
                get { throw new NotImplementedException(); }
            }

            public string SettableName { get; set; }
            public override string Name { get { return SettableName; } }

            public override Type DeclaringType
            {
                get { throw new NotImplementedException(); }
            }

            public override Type ReflectedType
            {
                get { throw new NotImplementedException(); }
            }

            public override RuntimeMethodHandle MethodHandle
            {
                get { throw new NotImplementedException(); }
            }

            public override MethodAttributes Attributes
            {
                get { throw new NotImplementedException(); }
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                return new object[0];
            }
        }

        private class ParameterInfoFake : ParameterInfo
        {
            public ParameterInfoFake()
            {
            }
        }
    }
}