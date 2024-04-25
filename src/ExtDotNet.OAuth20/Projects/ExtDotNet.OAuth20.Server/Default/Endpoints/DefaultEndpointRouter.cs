// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Endpoints;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Endpoints;

public class DefaultEndpointRouter(IEndpointProvider endpointProvider) : IEndpointRouter
{
    private const string RootPath = "/";

    private readonly IEndpointProvider _endpointProvider = endpointProvider ?? throw new ArgumentNullException(nameof(endpointProvider));

    public bool TryGetEndpoint(HttpContext httpContext, out IEndpoint? endPoint)
    {
        string endpointPath = httpContext.Request.Path.ToUriComponent();

        // TODO: handle prefixes
        if (endpointPath == RootPath)
        {
            endPoint = null;
            return false;
        }

        return _endpointProvider.TryGetEndpointInstanceByRoute(endpointPath, out endPoint);
    }
}
