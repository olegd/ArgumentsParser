using System.Collections.Generic;
using System.Reflection;

namespace ArgumentParser
{
    public interface IHandlersDiscoverer
    {
        IEnumerable<IHandlerDescriptor> GetHandlers();
    }
}