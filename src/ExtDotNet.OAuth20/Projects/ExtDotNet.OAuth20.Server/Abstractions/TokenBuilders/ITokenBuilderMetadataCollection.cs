// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;

public interface ITokenBuilderMetadataCollection
{
    public IDictionary<string, TokenBuilderMetadata> TokenBuilders { get; set; }
}
