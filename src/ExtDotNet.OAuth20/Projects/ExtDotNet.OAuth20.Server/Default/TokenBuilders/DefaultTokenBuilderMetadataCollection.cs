// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;
using System.Collections.Concurrent;

namespace ExtDotNet.OAuth20.Server.Default.TokenBuilders;

public class DefaultTokenBuilderMetadataCollection : ITokenBuilderMetadataCollection
{
    public IDictionary<string, TokenBuilderMetadata> TokenBuilders { get; set; } = new ConcurrentDictionary<string, TokenBuilderMetadata>();
}
