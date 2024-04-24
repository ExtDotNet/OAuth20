// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Endpoints;

public interface IEndpointProvider
{
    public bool TryGetEndpointInstanceByRoute(string route, out IEndpoint? endpoint);
}
