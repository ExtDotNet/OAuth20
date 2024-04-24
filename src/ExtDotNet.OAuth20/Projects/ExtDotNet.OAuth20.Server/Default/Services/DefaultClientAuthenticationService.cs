// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ClientSecretReaders;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultClientAuthenticationService : IClientAuthenticationService
{
    private readonly IClientSecretReaderSelector _clientSecretReaderSelector;
    private readonly IClientService _clientService;

    public DefaultClientAuthenticationService(IClientSecretReaderSelector clientSecretReaderSelector, IClientService clientService)
    {
        _clientSecretReaderSelector = clientSecretReaderSelector;
        _clientService = clientService;
    }

    public async Task<Client?> AuthenticateClientAsync(HttpContext httpContext)
    {
        var clientSecretReaders = await _clientSecretReaderSelector.GetClientSecretReadersAsync();

        if (!clientSecretReaders.Any())
        {
            throw new ServerConfigurationErrorException(
                "There isn't any Client Secret Readers configured for the Server. Client authentication is not possible." +
                "Please configure at least one Client Secret Reader or contact the Server Administrator about this issue.");
        }

        Client? client = null;

        var clientSecretReadersEnumerator = clientSecretReaders.GetEnumerator();

        try
        {
            while (clientSecretReadersEnumerator.MoveNext() && client is null)
            {
                IClientSecretReader clientSecretReader = clientSecretReadersEnumerator.Current;

                ClientSecret? clientSecret = await clientSecretReader.GetClientSecretAsync(httpContext);

                if (clientSecret is not null)
                {
                    client = await _clientService.GetClientAsync(clientSecret);
                }
            }
        }
        finally
        {
            clientSecretReadersEnumerator.Dispose();
        }

        return client;
    }
}
