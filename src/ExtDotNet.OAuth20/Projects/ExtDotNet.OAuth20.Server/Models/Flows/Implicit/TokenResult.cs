// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using System.Diagnostics;
using System.Text;

namespace ExtDotNet.OAuth20.Server.Models.Flows.Implicit.Mixed;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.2.2"/>
/// </summary>
public class TokenResult : TokenResultBase
{
    private TokenResult(
        string redirectUri,
        string accessToken,
        string tokenType,
        bool? expiresInRequired = false,
        long? expiresIn = null,
        string? scope = null,
        IDictionary<string, string?>? additionalParameters = null,
        string? state = null)
        : base(accessToken, tokenType, expiresIn, scope, additionalParameters)
    {
        if (expiresInRequired is true)
        {
            throw new ArgumentNullException(nameof(expiresIn));
        }

        RedirectUri = redirectUri;
        State = state;
    }

    public string RedirectUri { get; }

    public string? State { get; set; }

    public static TokenResult Create(
        string redirectUri,
        string accessToken,
        string tokenType,
        bool? expiresInRequired = false,
        long? expiresIn = null,
        string? scope = null,
        IDictionary<string, string?>? additionalParameters = null,
        string? state = null)
        => new(redirectUri,
            accessToken,
            tokenType,
            expiresInRequired,
            expiresIn,
            scope,
            additionalParameters,
            state);

    public override Task ExecuteAsync(HttpContext httpContext)
    {
        StringBuilder stringBuilder = new(RedirectUri);
        stringBuilder.AppendFormat("?access_token={0}", AccessToken);
        stringBuilder.AppendFormat("&token_type={0}", TokenType);

        if (ExpiresIn is not null)
        {
            stringBuilder.AppendFormat("&expires_in={0}", ExpiresIn);
        }

        if (Scope is not null)
        {
            stringBuilder.AppendFormat("&scope={0}", Scope);
        }

        if (State is not null)
        {
            stringBuilder.AppendFormat("&state={0}", State);
        }

        // NOTE: The authorization server MUST NOT issue a refresh token (With Implicit Grant).
        string redirectLocation = stringBuilder.ToString();
#if DEBUG
        Debug.WriteLine(redirectLocation);
#endif
        httpContext.Response.Redirect(redirectLocation, false, false);

        return Task.CompletedTask;
    }
}
