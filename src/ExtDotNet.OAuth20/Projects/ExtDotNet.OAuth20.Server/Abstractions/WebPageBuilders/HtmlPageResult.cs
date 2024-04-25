// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.WebPageBuilders;

public class HtmlPageResult : IResult
{
    private readonly string _htmlContent;

    public HtmlPageResult(string htmlContent)
    {
        _htmlContent = htmlContent;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        httpContext.Response.ContentType = "text/html";

        await httpContext.Response.WriteAsync(_htmlContent).ConfigureAwait(false);
    }
}
