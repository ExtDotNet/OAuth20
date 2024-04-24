// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Endpoints;

namespace ExtDotNet.OAuth20.Server.Default.Endpoints;

public class DefaultEndpointProvider : IEndpointProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEndpointMetadataCollection _endpointMetadataCollection;

    public DefaultEndpointProvider(IServiceProvider serviceProvider, IEndpointMetadataCollection endpointMetadataCollection)
    {
        _serviceProvider = serviceProvider;
        _endpointMetadataCollection = endpointMetadataCollection;
    }

    public bool TryGetEndpointInstanceByRoute(string route, out IEndpoint? endpoint)
    {
        EndpointMetadata? endpointMetadata = _endpointMetadataCollection.Endpoints.Values.FirstOrDefault(x => x.Route.StartsWith(route));

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
