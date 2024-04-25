// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions;
using ExtDotNet.OAuth20.Server.Abstractions.Errors;

using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions;
using ExtDotNet.OAuth20.Server.Abstractions.WebPageBuilders;

namespace ExtDotNet.OAuth20.Server.Middleware;

public class OAuth20ServerWebPagesMiddleware
{
    private readonly RequestDelegate _next;

    public OAuth20ServerWebPagesMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IWebPageBuilderRouter router, ITlsValidator tlsValidator, IErrorResultProvider errorResultProvider)
    {
        if (router.TryGetWebPageBuilder(httpContext, out IWebPageBuilder? webPageBuilder))
        {
            var validationResult = tlsValidator.TryValidate(httpContext);

            if (!validationResult.Success)
            {
                throw new InvalidOperationException(validationResult.Description);
            }

            IResult result;

            try
            {
                result = await webPageBuilder!.InvokeAsync(httpContext).ConfigureAwait(false);
            }
            catch (OAuth20Exception exception)
            {
                result = errorResultProvider.GetErrorResult(exception);
            }
            catch (Exception)
            {
                throw;
            }

            await result.ExecuteAsync(httpContext).ConfigureAwait(false);
        }
        else
        {
            await _next.Invoke(httpContext);
        }
    }
}
