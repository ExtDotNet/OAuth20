// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;

namespace ExtDotNet.OAuth20.Server.Models.Flows.ResourceOwnerPasswordCredentials.Token;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.3.2"/>
/// </summary>
public class TokenArguments : TokenArgumentsBase
{
    private TokenArguments(
        string username,
        string password,
        string grantType,
        string? redirectUri = null,
        string? scope = null)
        : base(grantType, redirectUri, scope)
    {
        Username = username;
        Password = password;
    }

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public static TokenArguments Create(
        string username,
        string password,
        string grantType,
        string? redirectUri = null,
        string? scope = null)
        => new(username, password, grantType, redirectUri, scope);

    public static TokenArguments Create(FlowArguments flowArguments)
    {
        flowArguments.Values.TryGetValue("redirect_uri", out string? redirectUri);
        flowArguments.Values.TryGetValue("scope", out string? scope);
        string username = flowArguments.Values["username"];
        string password = flowArguments.Values["password"];
        string grantType = flowArguments.Values["grant_type"];

        return new(username, password, grantType, redirectUri, scope);
    }
}
