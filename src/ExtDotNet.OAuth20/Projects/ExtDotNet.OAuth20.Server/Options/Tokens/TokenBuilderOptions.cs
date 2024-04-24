// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.Tokens;

/// <summary>
/// TODO: more advanced business validation.
/// </summary>
public class TokenBuilderOptions
{
    public string? Description { get; set; }

    public TokenBuilderTypeOptions? Abstraction { get; set; }

    public TokenBuilderTypeOptions? Implementation { get; set; }

    public IDictionary<string, string>? AdditionalParameters { get; set; }
}
