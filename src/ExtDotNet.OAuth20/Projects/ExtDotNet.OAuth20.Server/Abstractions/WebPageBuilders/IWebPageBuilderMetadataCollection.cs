// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.WebPageBuilders;

public interface IWebPageBuilderMetadataCollection
{
    public IDictionary<string, WebPageBuilderMetadata> WebPageBuilders { get; set; }
}
