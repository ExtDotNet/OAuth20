// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.ClientSecrets;

public class OAuth20ServerClientSecretsOptions
{
    public const string DefaultSection = "OAuth20Server:ClientSecrets";

    public string? AuthorizationHeaderBasicClientSecretTypeName { get; set; }

    public string? RequestBodyClientCredentialsClientSecretTypeName { get; set; }

    public string? TlsCertificateClientSecretTypeName { get; set; }

    public IEnumerable<ClientSecretTypeOptions>? ClientSecretTypes { get; set; }
}
