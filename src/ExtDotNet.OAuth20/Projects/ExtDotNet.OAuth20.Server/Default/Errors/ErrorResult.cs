// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Options;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace ExtDotNet.OAuth20.Server.Default.Errors;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1"/>
/// </summary>
public class ErrorResult : IErrorResult
{
    private ErrorResult(string error, string? errorDescription = null, string? errorUri = null, string? state = null)
    {
        Error = error;
        ErrorDescription = errorDescription;
        ErrorUri = errorUri;
        State = state;
    }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1
    /// </summary>
    [Description("error")]
    public string Error { get; set; } = default!;

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1
    /// </summary>
    [Description("error_description")]
    public string? ErrorDescription { get; set; }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1
    /// </summary>
    [Description("error_uri")]
    public string? ErrorUri { get; set; }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1
    /// </summary>
    [Description("state")]
    public string? State { get; set; }

    public static ErrorResult Create(string error, string? errorDescription = null, string? errorUri = null, string? state = null)
        => new(error, errorDescription, errorUri, state);

    public static ErrorResult Create(DefaultCommonErrorType defaultErrorType, string? errorDescription = null, string? errorUri = null, string? state = null, OAuth20ServerOptions? options = null)
        => Create(defaultErrorType.GetFieldNameAttributeValue(options), errorDescription, errorUri, state);

    public static ErrorResult Create(DefaultAuthorizeErrorType defaultErrorType, string? errorDescription = null, string? errorUri = null, string? state = null, OAuth20ServerOptions? options = null)
       => Create(defaultErrorType.GetFieldNameAttributeValue(options), errorDescription, errorUri, state);

    public static ErrorResult Create(DefaultTokenErrorType defaultErrorType, string? errorDescription = null, string? errorUri = null, string? state = null, OAuth20ServerOptions? options = null)
        => Create(defaultErrorType.GetFieldNameAttributeValue(options), errorDescription, errorUri, state);

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
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

        stringBuilder.AppendFormat("\"error\":\"{0}\"", Error);

        if (ErrorDescription is not null)
        {
            stringBuilder.AppendFormat(",\"error_description\":\"{0}\"", ErrorDescription);
        }

        if (ErrorUri is not null)
        {
            stringBuilder.AppendFormat(",\"error_uri\":\"{0}\"", ErrorUri);
        }

        if (State is not null)
        {
            stringBuilder.AppendFormat(",\"state\":\"{0}\"", State);
        }

        stringBuilder.Append('}');

        string responseBody = stringBuilder.ToString();
#if DEBUG
        Debug.WriteLine(responseBody);
#endif
        await httpContext.Response.WriteAsync(responseBody);
    }
}
