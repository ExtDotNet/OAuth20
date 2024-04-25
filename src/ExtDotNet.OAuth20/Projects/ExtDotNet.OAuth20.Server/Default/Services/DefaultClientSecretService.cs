// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultClientSecretService : IClientSecretService
{
    private readonly IClientSecretDataSource _clientSecretDataSource;

    public DefaultClientSecretService(IClientSecretDataSource clientSecretDataSource)
    {
        _clientSecretDataSource = clientSecretDataSource;
    }

    public async Task<ClientSecret?> GetClientSecretAsync(string type, string clientSecretContent) =>
         await _clientSecretDataSource.GetClientSecretAsync(type, clientSecretContent).ConfigureAwait(false);

    public async Task<ClientSecret?> GetEmptyClientSecretAsync(string type, Client client) =>
        await _clientSecretDataSource.GetEmptyClientSecretAsync(type, client).ConfigureAwait(false);
}
