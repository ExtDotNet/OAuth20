// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ClientSecretReaders;
using System.Collections.Concurrent;

namespace ExtDotNet.OAuth20.Server.Default.ClientSecrets;

public class DefaultClientSecretReaderMetadataCollection : IClientSecretReaderMetadataCollection
{
    public IDictionary<string, ClientSecretReaderMetadata> ClientSecretReaders { get; set; } = new ConcurrentDictionary<string, ClientSecretReaderMetadata>();
}
