using System;
using Microsoft.Practices.ServiceLocation;

namespace ArgumentParser
{
    public class DependencyResolver
    {
        public static IDependencyResolver Current { get; private set; }

        public static void SetResolver(object commonServiceLocator)
        {
            Current = new ServiceLocatorToDependencyResolverAdaptor((IServiceLocator)commonServiceLocator);
        }

        public static void SetResolver(IDependencyResolver dependencyResolver)
        {
            Current = dependencyResolver;
        }
    }

    public interface IDependencyResolver
    {
        object GetService(Type serviceType);
    }

    public class ServiceLocatorToDependencyResolverAdaptor : IDependencyResolver
    {
        private readonly IServiceLocator _serviceLocator;

        public ServiceLocatorToDependencyResolverAdaptor(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null) 
                throw new ArgumentNullException("serviceLocator");

            _serviceLocator = serviceLocator;
        }

        public object GetService(Type serviceType)
        {
            return _serviceLocator.GetInstance(serviceType);
        }
    }
}