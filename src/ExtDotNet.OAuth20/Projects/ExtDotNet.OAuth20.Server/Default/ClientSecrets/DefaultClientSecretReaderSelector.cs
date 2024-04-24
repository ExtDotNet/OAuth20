// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ClientSecretReaders;

namespace ExtDotNet.OAuth20.Server.Default.ClientSecrets;

public class DefaultClientSecretReaderSelector : IClientSecretReaderSelector
{
    private readonly IClientSecretReaderProvider _clientSecretReaderProvider;

    public DefaultClientSecretReaderSelector(IClientSecretReaderProvider clientSecretReaderProvider)
    {
        _clientSecretReaderProvider = clientSecretReaderProvider;
    }

    public Task<IEnumerable<IClientSecretReader>> GetClientSecretReadersAsync()
    {
        var readers = _clientSecretReaderProvider.GetAllClientSecretReaderInstances();

        return Task.FromResult(readers);
    }
}
