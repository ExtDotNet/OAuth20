// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Endpoints;

public abstract class RedirectResultBase : IResult
{
    protected RedirectResultBase(string redirectUri, IDictionary<string, string>? queryParameters = null)
    {
        RedirectUri = redirectUri;
        QueryParameters = queryParameters;
    }

    protected RedirectResultBase()
    {
    }

    public string RedirectUri { get; set; } = default!;

    public IDictionary<string, string>? QueryParameters { get; set; }

    public abstract Task ExecuteAsync(HttpContext httpContext);
}
