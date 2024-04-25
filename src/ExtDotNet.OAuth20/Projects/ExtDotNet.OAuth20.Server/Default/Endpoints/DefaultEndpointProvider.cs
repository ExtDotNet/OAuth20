// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Endpoints;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Endpoints;

public class DefaultEndpointProvider(IServiceProvider serviceProvider, IEndpointMetadataCollection endpointMetadataCollection) : IEndpointProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    private readonly IEndpointMetadataCollection _endpointMetadataCollection = endpointMetadataCollection ?? throw new ArgumentNullException(nameof(endpointMetadataCollection));

    public bool TryGetEndpointInstanceByRoute(string route, out IEndpoint? endpoint)
    {
        // TODO: handle prefixes
        _endpointMetadataCollection.Endpoints.TryGetValue(route, out EndpointMetadata? endpointMetadata);

        if (endpointMetadata is not null)
        {
            endpoint = (IEndpoint)_serviceProvider.GetRequiredService(endpointMetadata.Abstraction);
            return true;
        }
        else
        {
            endpoint = null;
            return false;
        }
    }
}
