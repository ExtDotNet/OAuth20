// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.Tokens;

public class OAuth20ServerTokensOptions
{
    public const string DefaultSection = "OAuth20Server:Tokens";

    public string? BasicTokenTypeName { get; set; }

    public string? JwtTokenTypeName { get; set; }

    public string? MacTokenTypeName { get; set; }

    public string? DefaultTokenType { get; set; }

    public string? ClientTokenOwnerTypeName { get; set; }

    public string? EndUserTokenOwnerTypeName { get; set; }

    public int? DefaultTokenExpirationSeconds { get; set; }

    public IEnumerable<TokenTypeOptions>? TokenTypes { get; set; }
}
