// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.WebPageBuilders;

public interface IWebPageBuilderProvider
{
    public bool TryGetWebPageBuilderInstanceByRoute(string route, out IWebPageBuilder? webPageBuilder);
}
