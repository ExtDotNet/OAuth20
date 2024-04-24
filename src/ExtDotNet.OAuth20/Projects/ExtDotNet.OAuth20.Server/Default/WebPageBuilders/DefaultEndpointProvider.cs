// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.WebPageBuilders;

namespace ExtDotNet.OAuth20.Server.Default.WebPageBuilders;

public class DefaultWebPageBuilderProvider : IWebPageBuilderProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWebPageBuilderMetadataCollection _webPageBuilderMetadataCollection;

    public DefaultWebPageBuilderProvider(IServiceProvider serviceProvider, IWebPageBuilderMetadataCollection webPageBuilderMetadataCollection)
    {
        _serviceProvider = serviceProvider;
        _webPageBuilderMetadataCollection = webPageBuilderMetadataCollection;
    }

    public bool TryGetWebPageBuilderInstanceByRoute(string route, out IWebPageBuilder? webPageBuilder)
    {
        WebPageBuilderMetadata? webPageBuilderMetadata = _webPageBuilderMetadataCollection.WebPageBuilders.Values.FirstOrDefault(x => x.Route.StartsWith(route));

        if (webPageBuilderMetadata is not null)
        {
            webPageBuilder = (IWebPageBuilder)_serviceProvider.GetRequiredService(webPageBuilderMetadata.Abstraction);
            return true;
        }
        else
        {
            webPageBuilder = null;
            return false;
        }
    }
}
