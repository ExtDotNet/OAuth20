// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Endpoints;

public interface IEndpointMetadataCollection
{
    public IDictionary<string, EndpointMetadata> Endpoints { get; set; }
}
