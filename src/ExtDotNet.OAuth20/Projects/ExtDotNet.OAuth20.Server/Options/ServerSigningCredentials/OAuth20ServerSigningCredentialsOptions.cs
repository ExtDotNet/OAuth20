// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.ServerSigningCredentials;

public class OAuth20ServerSigningCredentialsOptions
{
    public const string DefaultSection = "OAuth20Server:ServerSigningCredentials";

    public string? DeploymentTypePemName { get; set; }

    public string? DeploymentTypeEncryptedPemName { get; set; }

    public string? DeploymentTypePemFileName { get; set; }

    public string? DeploymentTypeEncryptedPemFileName { get; set; }

    public SelfSigningCredentialsOptions? SelfSigningCredentialsList { get; set; }

    public IEnumerable<X509Certificate2Options>? X509Certificate2List { get; set; }

    public string? DefaultSigningCredentialsAlgorithm { get; set; } = default!;
}
