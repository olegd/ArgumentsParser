using System.Collections.Generic;
using System.Reflection;

namespace ArgumentParser
{
    public interface IHandlersDiscoverer
    {
        List<IHandlerDescriptor> GetHandlers();
    }
}