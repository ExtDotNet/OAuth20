// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ClientSecretReaders;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.ClientSecretReaders;

public class DefaultClientSecretReaderProvider(IEnumerable<IClientSecretReader> clientSecretReaders) : IClientSecretReaderProvider
{
    private readonly IEnumerable<IClientSecretReader> _clientSecretReaders = clientSecretReaders ?? throw new ArgumentNullException(nameof(clientSecretReaders));

    public ValueTask<IEnumerable<IClientSecretReader>> GetAllClientSecretReadersAsync() => ValueTask.FromResult(_clientSecretReaders);
}
