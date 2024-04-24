// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Extensions;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Options;

public class OAuth20ServerOptionsValidator : IValidateOptions<OAuth20ServerOptions>
{
    public ValidateOptionsResult Validate(string? name, OAuth20ServerOptions options)
    {
        var validationResults = options.TryValidate();

        return validationResults.Count == 0 ?
            ValidateOptionsResult.Success :
            ValidateOptionsResult.Fail(validationResults.Select(x => x.ErrorMessage)!);
    }
}
