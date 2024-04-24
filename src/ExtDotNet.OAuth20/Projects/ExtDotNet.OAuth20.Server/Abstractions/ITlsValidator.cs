// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions;

public interface ITlsValidator
{
    public OAuth20ValidationResult TryValidate(HttpContext httpContext);
}
