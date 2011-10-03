using System;
using Microsoft.Practices.ServiceLocation;

namespace ArgumentParser.DependencyResolution
{
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