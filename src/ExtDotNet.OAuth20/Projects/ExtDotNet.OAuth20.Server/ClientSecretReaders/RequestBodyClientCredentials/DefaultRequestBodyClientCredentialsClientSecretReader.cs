// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Token;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Enums;

namespace ExtDotNet.OAuth20.Server.ClientSecretReaders.RequestBodyClientCredentials;

public class DefaultRequestBodyClientCredentialsClientSecretReader : IRequestBodyClientCredentialsClientSecretReader
{
    private readonly IClientService _clientService;
    private readonly IClientSecretService _clientSecretService;

    public DefaultRequestBodyClientCredentialsClientSecretReader(IClientService clientService, IClientSecretService clientSecretService)
    {
        _clientService = clientService;
        _clientSecretService = clientSecretService;
    }

    public async Task<ClientSecret?> GetClientSecretAsync(HttpContext httpContext)
    {
        ClientSecret? clientSecret = null;

        if (httpContext.Request.Method != HttpMethods.Post)
        {
            return clientSecret;
        }

        var values = httpContext.Request.Form.ToDictionary(x => x.Key, x => x.Value.First()!);

        if (!values.Any())
        {
            return clientSecret;
        }

        if (values.TryGetValue("client_id", out string? requestedClientId))
        {
            Client? client = await _clientService.GetClientAsync(requestedClientId);

            if (client is null)
            {
                throw new InvalidClientException($"Client with [client_id] = [{requestedClientId}] doesn't exist in the system.");
            }

            if (values.TryGetValue("client_secret", out string? requestedClientSecret))
            {
                clientSecret = await _clientSecretService.GetClientSecretAsync(DefaultClientSecretType.RequestBodyClientCredentials.GetFieldNameAttributeValue(), requestedClientSecret);
            }
            else
            {
                clientSecret = await _clientSecretService.GetEmptyClientSecretAsync(DefaultClientSecretType.RequestBodyClientCredentials.GetFieldNameAttributeValue(), client);
            }
        }

        return clientSecret;
    }
}
