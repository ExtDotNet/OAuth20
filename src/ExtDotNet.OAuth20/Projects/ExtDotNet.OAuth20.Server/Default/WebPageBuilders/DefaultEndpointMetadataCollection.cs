// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.WebPageBuilders;
using System.Collections.Concurrent;

namespace ExtDotNet.OAuth20.Server.Default.WebPageBuilders;

public class DefaultWebPageBuilderMetadataCollection : IWebPageBuilderMetadataCollection
{
    public IDictionary<string, WebPageBuilderMetadata> WebPageBuilders { get; set; } = new ConcurrentDictionary<string, WebPageBuilderMetadata>();
}
