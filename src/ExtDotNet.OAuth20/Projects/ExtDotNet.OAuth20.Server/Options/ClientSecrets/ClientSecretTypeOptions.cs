// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.ClientSecrets;

public class ClientSecretTypeOptions
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public ClientSecretReaderOptions? Reader { get; set; }
}
