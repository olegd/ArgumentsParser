using System.Collections.Generic;
using ArgumentParser.Routing;

namespace ArgumentParser.Handling
{
    public interface IHandlerInvoker
    {
        void Invoke(IHandler handler, string[] args);
        Dictionary<string, object> MapArguments(IHandler handler, string[] args);
    }
}