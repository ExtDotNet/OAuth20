// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions;

namespace ExtDotNet.OAuth20.Server.Endpoints.Token;

public class DefaultTokenRequestValidator : IRequestValidator<ITokenEndpoint>
{
    public OAuth20ValidationResult TryValidate(HttpContext httpContext)
    {
        OAuth20ValidationResult result = new();

        if (httpContext.Request.Method == HttpMethod.Post.Method) result.Success = true;

        return result;
    }
}
