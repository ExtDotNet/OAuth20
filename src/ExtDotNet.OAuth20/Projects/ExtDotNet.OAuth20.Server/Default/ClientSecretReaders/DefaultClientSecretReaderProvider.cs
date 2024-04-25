// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ClientSecretReaders;

namespace ExtDotNet.OAuth20.Server.Default.ClientSecretReaders;

public class DefaultClientSecretReaderProvider : IClientSecretReaderProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IClientSecretReaderMetadataCollection _clientSecretReaderMetadataCollection;

    public DefaultClientSecretReaderProvider(IServiceProvider serviceProvider, IClientSecretReaderMetadataCollection clientSecretReaderMetadataCollection)
    {
        _serviceProvider = serviceProvider;
        _clientSecretReaderMetadataCollection = clientSecretReaderMetadataCollection;
    }

    public IEnumerable<IClientSecretReader> GetAllClientSecretReaderInstances()
    {
        foreach (var clientSecretReaderMetadata in _clientSecretReaderMetadataCollection.ClientSecretReaders)
        {
            yield return (IClientSecretReader)_serviceProvider.GetRequiredService(clientSecretReaderMetadata.Value.Abstraction);
        }
    }
}
