// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Enums;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.ClientSecretReaders.TlsCertificate;

public class DefaultTlsCertificateClientSecretReader(IClientSecretService clientSecretService) : ITlsCertificateClientSecretReader
{
    private readonly IClientSecretService _clientSecretService = clientSecretService ?? throw new ArgumentNullException(nameof(clientSecretService));

    public async Task<ClientSecret?> GetClientSecretAsync(HttpContext httpContext)
    {
        ClientSecret? clientSecret = null;

        var clientCertificate = await httpContext.Connection
            .GetClientCertificateAsync()
            .ConfigureAwait(false);

        if (clientCertificate is null) return clientSecret;

        string clientSecretContent = clientCertificate.GetRawCertDataString();

        await _clientSecretService
            .GetClientSecretAsync(DefaultClientSecretType.TlsCertificate.GetFieldNameAttributeValue(), clientSecretContent)
            .ConfigureAwait(false);

        return clientSecret;
    }
}
