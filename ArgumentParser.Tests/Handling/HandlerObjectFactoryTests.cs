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
        public void Invoke_HandlerIsInstanceMemberWithConstructorThatHasOneArgument_ArgumentTypeCanBeResolvedThroughServiceLocator_HandlerIsInvoked()
        {
            //Arrange
            var handlerMethod = typeof(HandlerHostWithoutDefaultCtor).GetMethod("Merge");

            var serviceLocator = new ServiceLocatorForTest();

            var dependencyObject = serviceLocator.Register(typeof(IDependency), new Dependency());
            DependencyResolver.SetResolver(serviceLocator);

            //Act
            var handlerObject = HandlerObjectFactory.Create(handlerMethod);

            //Assert
            Assert.IsNotNull(handlerObject);
            Assert.IsInstanceOf<HandlerHostWithoutDefaultCtor>(handlerObject);
            Assert.AreEqual(dependencyObject, ((HandlerHostWithoutDefaultCtor)handlerObject).Dependency);
        }
        
        [Test]
        public void Invoke_HandlerIsInstanceMemberWithTwoConstructorsThatHaveArgumentsThatCanBeResolvedThroughServiceLocator_HandlerIsInvoked()
        {
            //Arrange
            var handlerMethod = typeof(HandlerHostWithTwoConstructors).GetMethod("Merge");

            var serviceLocator = new ServiceLocatorForTest();
            var dependency = serviceLocator.Register(typeof(IDependency), new Dependency());
            var dependency2 = serviceLocator.Register(typeof(IDependency2), new Dependency2());
            DependencyResolver.SetResolver(serviceLocator);
            

            //Act
            var handlerObject = HandlerObjectFactory.Create(handlerMethod);

            //Assert
            Assert.IsNotNull(handlerObject);
            Assert.IsInstanceOf<HandlerHostWithTwoConstructors>(handlerObject);
            var castedHandlerObject = (HandlerHostWithTwoConstructors)handlerObject;
            Assert.AreEqual(dependency, castedHandlerObject.Dependency);
            Assert.AreEqual(dependency2, castedHandlerObject.Dependency2);
        }

        [Test]
        public void Invoke_HandlerIsInstanceMemberWithConstructorThatHasOneArgument_ArgumentTypeCanBeResolvedThroughDependencyResolver_HandlerIsInvoked()
        {
            //Arrange
            var handlerMethod = typeof(HandlerHostWithoutDefaultCtor).GetMethod("Merge");

            var dependencyResolver = new DependencyResolverForTest();
            var dependencyObject = dependencyResolver.Register(typeof(IDependency), new Dependency());
            DependencyResolver.SetResolver(dependencyResolver);

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

        private class HandlerHostWithTwoConstructors
        {
            public IDependency Dependency { get; set; }
            public IDependency2 Dependency2 { get; set; }

            public HandlerHostWithTwoConstructors(IDependency dependency)
            {
                Dependency = dependency;
            }

            public HandlerHostWithTwoConstructors(IDependency dependency, IDependency2 dependency2)
            {
                Dependency = dependency;
                Dependency2 = dependency2;
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

        private interface IDependency2
        {
        }

        private class Dependency2 : IDependency2
        {
        }

        private class Dependency3 : IDependency3
        {
        }

        private interface IDependency3
        {
        }

        public class DependencyResolverForTest : IDependencyResolver
        {
            private readonly Dictionary<Type, object> _instanceForType = new Dictionary<Type, object>();
           
            public object Register(Type serviceType, object instance)
            {
                _instanceForType[serviceType] = instance;
                return instance;
            }

            public object GetService(Type serviceType)
            {
                return _instanceForType[serviceType];
            }
        }


        #region simple Service Locator implementation
        public class ServiceLocatorForTest : IServiceLocator
        {
            private Dictionary<Type, object> _instanceForType = new Dictionary<Type, object>();

            public object Register(Type type, object instance)
            {
                _instanceForType[type] = instance;
                return instance;
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
        #endregion
    }
}