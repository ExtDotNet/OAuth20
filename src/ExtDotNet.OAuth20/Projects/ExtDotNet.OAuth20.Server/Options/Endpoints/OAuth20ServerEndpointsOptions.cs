// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.Endpoints;

public class OAuth20ServerEndpointsOptions
{
    public const string DefaultSection = "OAuth20Server:Endpoints";

    public bool AuthorizationEndpointHttpPostEnabled { get; set; } = false;

    public string? AuthorizationEndpointRoute { get; set; }

    public string? TokenEndpointRoute { get; set; }

    public IEnumerable<EndpointOptions>? EndpointList { get; set; }
}
