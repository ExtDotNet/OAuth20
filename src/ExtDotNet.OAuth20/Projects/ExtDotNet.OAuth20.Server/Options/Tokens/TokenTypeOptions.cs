// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.Tokens;

public class TokenTypeOptions
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public TokenBuilderOptions? Builder { get; set; }

    public IDictionary<string, string>? AdditionalParameters { get; set; }
}
