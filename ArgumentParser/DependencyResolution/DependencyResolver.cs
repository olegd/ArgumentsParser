using Microsoft.Practices.ServiceLocation;

namespace ArgumentParser.DependencyResolution
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
}

