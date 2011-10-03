using System;

namespace ArgumentParser
{
    public interface IDependencyResolver
    {
        object GetService(Type serviceType);
    }
}