// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Endpoints.Authorization;

public class DefaultAuthorizationRequestValidator(IOptions<OAuth20ServerOptions> options) : IRequestValidator<IAuthorizationEndpoint>
{
    private readonly IOptions<OAuth20ServerOptions> _options = options ?? throw new ArgumentNullException(nameof(options));

    public OAuth20ValidationResult TryValidate(HttpContext httpContext)
    {
        OAuth20ValidationResult result = new();

        if (httpContext.Request.Method == HttpMethod.Get.Method)
        {
            result.Success = true;
        }
        else if (_options.Value.Endpoints?.AuthorizationEndpointHttpPostEnabled == true && httpContext.Request.Method == HttpMethod.Post.Method)
        {
            result.Success = true;
        }
        else
        {
            result.Success = false;
        }

        return result;
    }
}
