// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Endpoints;
using ExtDotNet.OAuth20.Server.Models.Flows;

namespace ExtDotNet.OAuth20.Server.Models;

public class LoginRedirectResult : RedirectResultBase
{
    public LoginRedirectResult(string loginEndpoint)
        : base(loginEndpoint)
    {
    }

    public LoginRedirectResult(string loginEndpoint, FlowArguments flowArguments, IDictionary<string, string>? additionalParameters = null)
        : base(loginEndpoint)
    {
        QueryParameters = flowArguments.Values;

        if (additionalParameters is not null && additionalParameters.Any())
        {
            QueryParameters = QueryParameters.Concat(additionalParameters).ToDictionary();
        }

        QueryParameters["oauth20_server_redirect"] = "/oauth/authorize";
    }
}
