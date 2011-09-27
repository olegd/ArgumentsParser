using System.Collections.Generic;
using ArgumentParser.Routing;

namespace ArgumentParser.Configuration
{
    public interface IHandlerProvider
    {
        List<IHandler> GetHandlers();
    }
}