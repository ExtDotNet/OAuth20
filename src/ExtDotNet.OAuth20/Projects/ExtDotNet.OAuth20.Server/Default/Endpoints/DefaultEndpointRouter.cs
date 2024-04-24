// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Endpoints;

namespace ExtDotNet.OAuth20.Server.Default.Endpoints;

public class DefaultEndpointRouter : IEndpointRouter
{
    private readonly IEndpointProvider _endpointProvider;

    public DefaultEndpointRouter(IEndpointProvider endpointProvider)
    {
        _endpointProvider = endpointProvider;
    }

    public bool TryGetEndpoint(HttpContext httpContext, out IEndpoint? endPoint)
    {
        string endpointPath = httpContext.Request.Path.ToUriComponent();

        if (endpointPath == "/")
        {
            endPoint = null;
            return false;
        }

        return _endpointProvider.TryGetEndpointInstanceByRoute(endpointPath, out endPoint);
    }
}
