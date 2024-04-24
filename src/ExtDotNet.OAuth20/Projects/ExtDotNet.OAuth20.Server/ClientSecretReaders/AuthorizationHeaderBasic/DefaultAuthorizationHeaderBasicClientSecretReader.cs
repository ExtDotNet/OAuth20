// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Token;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Enums;
using System.Text;

namespace ExtDotNet.OAuth20.Server.ClientSecretReaders.AuthorizationHeaderBasic;

public class DefaultAuthorizationHeaderBasicClientSecretReader : IAuthorizationHeaderBasicClientSecretReader
{
    private readonly IClientService _clientService;
    private readonly IClientSecretService _clientSecretService;

    public DefaultAuthorizationHeaderBasicClientSecretReader(IClientService clientService, IClientSecretService clientSecretService)
    {
        _clientService = clientService;
        _clientSecretService = clientSecretService;
    }

    public async Task<ClientSecret?> GetClientSecretAsync(HttpContext httpContext)
    {
        ClientSecret? clientSecret = null;

        if (!httpContext.Request.Headers.TryGetValue("Authorization", out var values))
        {
            return clientSecret;
        }

        string? headerValue = values.FirstOrDefault();

        if (headerValue == null)
        {
            return clientSecret;
        }

        if (!headerValue.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            return clientSecret;
        }

        string headerValueString;

        try
        {
            byte[] headerValueBytes = Convert.FromBase64String(headerValue);
            headerValueString = Encoding.UTF8.GetString(headerValueBytes);
        }
        catch (FormatException)
        {
            throw new Abstractions.Errors.Exceptions.Common.InvalidRequestException(
                "Request has [Authentication] header with a value starts with [Basic] substring " +
                "but the rest substring of the value malformed. The correct format is [Basic base64string] " +
                "where [base64string] is a Base64Encoded string with 2 values: Client Id and Client Secret " +
                "which separated with [:] symbol. " +
                "RFC2617 (HTTP Authentication: Basic and Digest Access Authentication): https://datatracker.ietf.org/doc/html/rfc2617, " +
                "RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-2.3.1");
        }

        string[] clientCredentials = headerValueString.Split(':');

        if (clientCredentials.Length == 0 || clientCredentials.Length > 2)
        {
            throw new Abstractions.Errors.Exceptions.Common.InvalidRequestException(
                "Request has [Authentication] header with a value starts with [Basic] substring " +
                "and the rest correct substring of the Base64Encoded value but it seems that after " +
                "decoding this value malformed. The correct format of decoded string is a string " +
                "with 2 values: Client Id and Client Secret which separated with [:] symbol, or " +
                "with 1 value: Client Id without any [:] symbol. " +
                "RFC2617 (HTTP Authentication: Basic and Digest Access Authentication): https://datatracker.ietf.org/doc/html/rfc2617, " +
                "RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-2.3.1");
        }

        string requestedClientId = clientCredentials[0];
        string? requestedClientSecret = clientCredentials.Length == 2 ? clientCredentials[1] : null;

        Client? client = await _clientService.GetClientAsync(requestedClientId);

        if (client is null)
        {
            throw new InvalidClientException($"Client with [client_id] = [{requestedClientId}] doesn't exist in the system.");
        }

        if (requestedClientSecret is not null)
        {
            clientSecret = await _clientSecretService.GetClientSecretAsync(DefaultClientSecretType.AuthorizationHeaderBasic.GetFieldNameAttributeValue(), requestedClientSecret);
        }
        else
        {
            clientSecret = await _clientSecretService.GetEmptyClientSecretAsync(DefaultClientSecretType.AuthorizationHeaderBasic.GetFieldNameAttributeValue(), client);
        }

        return clientSecret;
    }
}
