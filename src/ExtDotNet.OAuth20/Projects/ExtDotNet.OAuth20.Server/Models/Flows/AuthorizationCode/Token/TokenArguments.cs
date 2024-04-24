// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;

namespace ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Token;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.3"/>
/// </summary>
public class TokenArguments : TokenArgumentsBase
{
    private TokenArguments(
        string code,
        string grantType,
        string? redirectUri = null,
        string? scope = null)
        : base(grantType, redirectUri, scope)
    {
        Code = code;
    }

    public string Code { get; set; } = default!;

    public static TokenArguments Create(
        string code,
        string grantType,
        string? redirectUri = null,
        string? scope = null)
        => new(code, grantType, redirectUri, scope);

    public static TokenArguments Create(FlowArguments flowArguments)
    {
        flowArguments.Values.TryGetValue("redirect_uri", out string? redirectUri);
        flowArguments.Values.TryGetValue("scope", out string? scope);
        string code = flowArguments.Values["code"];
        string grantType = flowArguments.Values["grant_type"];

        return new(code, grantType, redirectUri, scope);
    }
}
