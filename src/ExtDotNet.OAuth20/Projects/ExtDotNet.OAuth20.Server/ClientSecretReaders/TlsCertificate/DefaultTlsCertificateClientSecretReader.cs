// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Enums;

namespace ExtDotNet.OAuth20.Server.ClientSecretReaders.TlsCertificate;

public class DefaultTlsCertificateClientSecretReader : ITlsCertificateClientSecretReader
{
    private readonly IClientSecretService _clientSecretService;

    public DefaultTlsCertificateClientSecretReader(IClientSecretService clientSecretService)
    {
        _clientSecretService = clientSecretService;
    }

    public async Task<ClientSecret?> GetClientSecretAsync(HttpContext httpContext)
    {
        ClientSecret? clientSecret = null;

        var clientCertificate = await httpContext.Connection.GetClientCertificateAsync();

        if (clientCertificate is null)
        {
            return clientSecret;
        }

        string clientSecretContent = clientCertificate.GetRawCertDataString();

        await _clientSecretService.GetClientSecretAsync(DefaultClientSecretType.TlsCertificate.GetFieldNameAttributeValue(), clientSecretContent);

        return clientSecret;
    }
}
