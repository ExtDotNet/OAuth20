// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.ServerSigningCredentials;

public class SelfSigningCredentialsOptions
{
    public const string DefaultSection = "OAuth20Server:ServerSigningCredentials:SelfSigningCredentialsList";

    public string? RsaSha256Name { get; set; }

    public string? RsaSha384Name { get; set; }

    public string? RsaSha512Name { get; set; }

    public string? RsaSsaPssSha256Name { get; set; }

    public string? RsaSsaPssSha384Name { get; set; }

    public string? RsaSsaPssSha512Name { get; set; }

    public string? EcdsaSha256Name { get; set; }

    public string? EcdsaSha384Name { get; set; }

    public string? EcdsaSha512Name { get; set; }

    public IEnumerable<SecurityKeyOptions>? SecurityKeys { get; set; }
}
