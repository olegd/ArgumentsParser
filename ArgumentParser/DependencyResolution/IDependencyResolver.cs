using System;

namespace ArgumentParser.DependencyResolution
{
    public interface IDependencyResolver
    {
        object GetService(Type serviceType);
    }
}