// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.ClientSecretReaders;

public class ClientSecretReaderMetadata
{
    protected ClientSecretReaderMetadata(string clientSecretType, Type abstraction, string? description = null)
    {
        ClientSecretType = clientSecretType;
        Abstraction = abstraction;
        Description = description;
    }

    public virtual string ClientSecretType { get; set; } = default!;

    public virtual Type Abstraction { get; set; } = default!;

    public virtual string? Description { get; set; }

    public static ClientSecretReaderMetadata Create<TAbstraction>(string clientSecretType, string? description = null)
        where TAbstraction : IClientSecretReader
        => Create(clientSecretType, typeof(TAbstraction), description);

    public static ClientSecretReaderMetadata Create(string clientSecretType, Type abstraction, string? description = null)
    {
        if (!abstraction.IsAssignableTo(typeof(IClientSecretReader)))
        {
            throw new ArgumentException(nameof(abstraction));
        }

        return new(clientSecretType, abstraction, description);
    }
}
