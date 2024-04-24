// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Flows;

public abstract class AuthorizeArgumentsBase
{
    protected AuthorizeArgumentsBase(
        string responseType,
        string clientId,
        string? redirectUri = null,
        string? scope = null,
        string? state = null)
    {
        ResponseType = responseType;
        ClientId = clientId;
        RedirectUri = redirectUri;
        Scope = scope;
        State = state;
    }

    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.1.1"/>
    /// </summary>
    public string ResponseType { get; set; } = default!;

    public string ClientId { get; set; } = default!;

    public string? RedirectUri { get; set; }

    public string? Scope { get; set; }

    public string? State { get; set; }
}
