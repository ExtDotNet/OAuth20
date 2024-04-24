// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.ClientSecrets;

/// <summary>
/// TODO: more advanced business validation.
/// </summary>
public class ClientSecretReaderOptions
{
    public string? Description { get; set; }

    public ClientSecretReaderTypeOptions? Abstraction { get; set; }

    public ClientSecretReaderTypeOptions? Implementation { get; set; }
}
