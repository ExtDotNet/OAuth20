// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;

namespace ExtDotNet.OAuth20.Server.Models.Flows.RefreshToken.Token;

/// <summary>
/// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-6
/// </summary>
public class TokenArguments : TokenArgumentsBase
{
    private TokenArguments(
        string refreshToken,
        string grantType,
        string? redirectUri = null,
        string? scope = null)
        : base(grantType, redirectUri, scope)
    {
        RefreshToken = refreshToken;
    }

    public string RefreshToken { get; set; } = default!;

    public static TokenArguments Create(
        string refreshToken,
        string grantType,
        string? redirectUri = null,
        string? scope = null)
        => new(refreshToken, grantType, redirectUri, scope);

    public static TokenArguments Create(FlowArguments flowArguments)
    {
        flowArguments.Values.TryGetValue("redirect_uri", out string? redirectUri);
        flowArguments.Values.TryGetValue("scope", out string? scope);
        string refreshToken = flowArguments.Values["refresh_token"];
        string grantType = flowArguments.Values["grant_type"];

        return new(refreshToken, grantType, redirectUri, scope);
    }
}
