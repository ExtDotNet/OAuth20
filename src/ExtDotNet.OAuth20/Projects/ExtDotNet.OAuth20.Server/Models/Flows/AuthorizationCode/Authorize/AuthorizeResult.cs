// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Abstractions.ServerInformation;
using System.Diagnostics;
using System.Text;

namespace ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Authorize;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2"/>
/// </summary>
public class AuthorizeResult : AuthorizeResultBase
{
    public AuthorizeResult(string redirectUri, string code, string? state = null)
        : base(state)
    {
        RedirectUri = redirectUri;
        Code = code;
    }

    public string RedirectUri { get; }

    /// <summary>
    /// The client should avoid making assumptions about code
    /// value sizes. The authorization server SHOULD document the size of
    /// any value it issues. All information should be provided by a
    /// <see cref="IServerInformationService"/> instance.
    /// </summary>
    public string Code { get; set; } = default!;

    public static AuthorizeResult Create(
        string redirectUri,
        string code,
        string? state = null)
    => new(redirectUri, code, state);

    public override Task ExecuteAsync(HttpContext httpContext)
    {
        StringBuilder stringBuilder = new(RedirectUri);
        stringBuilder.AppendFormat("?code={0}", Code);
        if (State is not null)
        {
            stringBuilder.AppendFormat("&state={0}", State);
        }

        string redirectLocation = stringBuilder.ToString();
#if DEBUG
        Debug.WriteLine(redirectLocation);
#endif
        httpContext.Response.Redirect(redirectLocation, false, false);

        return Task.CompletedTask;
    }
}
