// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using System.Diagnostics;
using System.Text;

namespace ExtDotNet.OAuth20.Server.Models.Flows.RefreshToken.Token;

/// <summary>
/// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-5.1
/// </summary>
public class TokenResult : TokenResultBase
{
    private TokenResult(
        string accessToken,
        string tokenType,
        bool? expiresInRequired = false,
        long? expiresIn = null,
        string? scope = null,
        IDictionary<string, string?>? additionalParameters = null,
        string? refreshToken = null)
        : base(accessToken, tokenType, expiresIn, scope, additionalParameters)
    {
        if (expiresInRequired is true)
        {
            throw new ArgumentNullException(nameof(expiresIn));
        }

        RefreshToken = refreshToken;
    }

    public string? RefreshToken { get; set; }

    public static TokenResult Create(
        string accessToken,
        string tokenType,
        bool? expiresInRequired = false,
        long? expiresIn = null,
        string? scope = null,
        IDictionary<string, string?>? additionalParameters = null,
        string? refreshToken = null)
        => new(
            accessToken,
            tokenType,
            expiresInRequired,
            expiresIn,
            scope,
            additionalParameters,
            refreshToken);

    public override async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        httpContext.Response.ContentType = "application/json;charset=UTF-8";

        if (!httpContext.Response.Headers.ContainsKey("Cache-Control"))
        {
            httpContext.Response.Headers.Append("Cache-Control", "no-store, no-cache, max-age=0");
        }
        else
        {
            httpContext.Response.Headers["Cache-Control"] = "no-store, no-cache, max-age=0";
        }

        if (!httpContext.Response.Headers.ContainsKey("Pragma"))
        {
            httpContext.Response.Headers.Append("Pragma", "no-cache");
        }
        else
        {
            httpContext.Response.Headers["Pragma"] = "no-cache";
        }

        StringBuilder stringBuilder = new('{');

        stringBuilder.AppendFormat("\"access_token\":\"{0}\"", AccessToken);
        stringBuilder.AppendFormat(",\"token_type\":\"{0}\"", TokenType);

        if (ExpiresIn is not null)
        {
            stringBuilder.AppendFormat(",\"expires_in\":{0}", ExpiresIn);
        }

        if (RefreshToken is not null)
        {
            stringBuilder.AppendFormat(",\"refresh_token\":\"{0}\"", RefreshToken);
        }

        if (AdditionalParameters is not null && AdditionalParameters.Count > 0)
        {
            foreach (var parameter in AdditionalParameters)
            {
                stringBuilder.AppendFormat(",\"{0}\":\"{1}\"", parameter.Key, parameter.Value);
            }
        }

        stringBuilder.Append('}');

        string responseBody = stringBuilder.ToString();
#if DEBUG
        Debug.WriteLine(responseBody);
#endif
        await httpContext.Response.WriteAsync(responseBody);
    }
}
