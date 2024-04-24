// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Endpoints;
using System.Collections.Concurrent;

namespace ExtDotNet.OAuth20.Server.Default.Endpoints;

public class DefaultEndpointMetadataCollection : IEndpointMetadataCollection
{
    public IDictionary<string, EndpointMetadata> Endpoints { get; set; } = new ConcurrentDictionary<string, EndpointMetadata>();
}
