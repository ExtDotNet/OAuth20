// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions;

namespace ExtDotNet.OAuth20.Server.Default;

public class DefaultTlsValidator : ITlsValidator
{
    public OAuth20ValidationResult TryValidate(HttpContext httpContext) => new() { Success = true };
}
