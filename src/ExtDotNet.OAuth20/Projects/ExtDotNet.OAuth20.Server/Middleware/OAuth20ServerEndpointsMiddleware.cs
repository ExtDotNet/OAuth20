// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions;
using ExtDotNet.OAuth20.Server.Abstractions.Endpoints;
using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions;

namespace ExtDotNet.OAuth20.Server.Middleware;

public class OAuth20ServerEndpointsMiddleware
{
    private readonly RequestDelegate _next;

    public OAuth20ServerEndpointsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IEndpointRouter router, ITlsValidator tlsValidator, IErrorResultProvider errorResultProvider)
    {
        if (router.TryGetEndpoint(httpContext, out IEndpoint? endpoint))
        {
            var validationResult = tlsValidator.TryValidate(httpContext);

            if (!validationResult.Success)
            {
                throw new InvalidOperationException(validationResult.Description);
            }

            IResult result;

            try
            {
                result = await endpoint!.InvokeAsync(httpContext);
            }
            catch (OAuth20Exception exception)
            {
                result = errorResultProvider.GetErrorResult(exception);
            }
            catch (Exception)
            {
                throw;
            }

            await result.ExecuteAsync(httpContext);
        }
        else
        {
            await _next.Invoke(httpContext);
        }
    }
}
