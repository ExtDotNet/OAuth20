// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.ClientSecretReaders;

public interface IClientSecretReaderMetadataCollection
{
    public IDictionary<string, ClientSecretReaderMetadata> ClientSecretReaders { get; set; }
}
