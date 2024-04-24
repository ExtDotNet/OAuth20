// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.WebPageBuilders;

namespace ExtDotNet.OAuth20.Server.Default.WebPageBuilders;

public class DefaultWebPageBuilderRouter : IWebPageBuilderRouter
{
    private readonly IWebPageBuilderProvider _webPageBuilderProvider;

    public DefaultWebPageBuilderRouter(IWebPageBuilderProvider webPageBuilderProvider)
    {
        _webPageBuilderProvider = webPageBuilderProvider;
    }

    public bool TryGetWebPageBuilder(HttpContext httpContext, out IWebPageBuilder? webPageBuilder)
    {
        string webPagePath = httpContext.Request.Path.ToUriComponent();

        if (webPagePath == "/")
        {
            webPageBuilder = null;
            return false;
        }

        return _webPageBuilderProvider.TryGetWebPageBuilderInstanceByRoute(webPagePath, out webPageBuilder);
    }
}
