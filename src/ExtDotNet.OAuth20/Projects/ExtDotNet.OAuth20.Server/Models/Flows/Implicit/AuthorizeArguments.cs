// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;

namespace ExtDotNet.OAuth20.Server.Models.Flows.Implicit.Mixed;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.2.1"/>
/// </summary>
public class AuthorizeArguments : AuthorizeArgumentsBase
{
    private AuthorizeArguments(
        string responseType,
        string clientId,
        string? redirectUri = null,
        string? scope = null,
        string? state = null)
        : base(responseType, clientId, redirectUri, scope, state)
    {
    }

    public static AuthorizeArguments Create(
        string responseType,
        string clientId,
        string? redirectUri = null,
        string? scope = null,
        string? state = null)
    => new(responseType, clientId, redirectUri, scope, state);

    public static AuthorizeArguments Create(FlowArguments flowArguments)
    {
        flowArguments.Values.TryGetValue("redirect_uri", out string? redirectUri);
        flowArguments.Values.TryGetValue("scope", out string? scope);
        flowArguments.Values.TryGetValue("state", out string? state);
        string responseType = flowArguments.Values["response_type"];
        string clientId = flowArguments.Values["client_id"];

        return new(responseType, clientId, redirectUri, scope, state);
    }
}
