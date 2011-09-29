using System;
using System.Collections.Generic;
using ArgumentParser.Handling;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;

namespace ArgumentParser.Tests.Handling
{
    [TestFixture]
    public class HandlerObjectFactoryTests
    {
        [Test]
        public void CreateObject_MethodIsStatic_ReturnsNull()
        {
            var handlerMethod = typeof(StaticHandlers).GetMethod("Merge");

            var handlerObject = HandlerObjectFactory.Create(handlerMethod);

            Assert.IsNull(handlerObject);
        }

        [Test]
        public void Invoke_HandlerIsInstanceMemberOnObjectWithDefaultConstructor_HandlerIsInvoked()
        {
            var handlerMethod = typeof(HandlerHostWithDefaultCtor).GetMethod("Merge");

            var handlerObject = HandlerObjectFactory.Create(handlerMethod);

            Assert.IsNotNull(handlerObject);
        }

        [Test]
        public void Invoke_HandlerIsInstanceMemberWithoutConstructorThatHasOneArgument_ArgumentTypeCanBeResolvedThroughServiceLocator_HandlerIsInvoked()
        {
            //Arrange
            var handlerMethod = typeof(HandlerHostWithoutDefaultCtor).GetMethod("Merge");

            var dependencyResolver = new TestDependencyServiceLocator();
            var dependencyObject = new Dependency();
            dependencyResolver.Register(typeof(IDependency), dependencyObject);
            ServiceLocator.SetLocatorProvider(() => dependencyResolver);

            //Act
            var handlerObject = HandlerObjectFactory.Create(handlerMethod);

            //Assert
            Assert.IsNotNull(handlerObject);
            Assert.IsInstanceOf<HandlerHostWithoutDefaultCtor>(handlerObject);
            Assert.AreEqual(dependencyObject, ((HandlerHostWithoutDefaultCtor)handlerObject).Dependency);
        }

        private class StaticHandlers
        {
            public static void Merge()
            {
            }
        }

        private class HandlerHostWithDefaultCtor
        {
            public void Merge()
            {
            }
        }

        private class HandlerHostWithoutDefaultCtor
        {
            public IDependency Dependency { get; set; }

            public HandlerHostWithoutDefaultCtor(IDependency dependency)
            {
                this.Dependency = dependency;
            }

            public void Merge()
            {
            }
        }

        private interface IDependency
        {
        }

        private class Dependency : IDependency
        {
        }

        public class TestDependencyServiceLocator : IServiceLocator
        {
            private Dictionary<Type, object> _instanceForType = new Dictionary<Type, object>();

            public void Register(Type type, object instance)
            {
                _instanceForType[type] = instance;
            }

            public object GetService(Type serviceType)
            {
                return GetInstance(serviceType);
            }

            public object GetInstance(Type serviceType)
            {
                return _instanceForType[serviceType];
            }

            public object GetInstance(Type serviceType, string key)
            {
                return GetInstance(serviceType);
            }

            public IEnumerable<object> GetAllInstances(Type serviceType)
            {
                return new List<object> { GetInstance(serviceType) };
            }

            public TService GetInstance<TService>()
            {
                return (TService)GetInstance(typeof(TService));
            }

            public TService GetInstance<TService>(string key)
            {
                return GetInstance<TService>();
            }

            public IEnumerable<TService> GetAllInstances<TService>()
            {
                return new List<TService> { GetInstance<TService>() };
            }
        }
    }
}