// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Services;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultServerMetadataService : IServerMetadataService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DefaultServerMetadataService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<Uri> GetCurrentInstanceUriAsync()
    {
        HttpRequest request = _httpContextAccessor.HttpContext!.Request;

        UriBuilder uriBuilder = new()
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port ?? (request.IsHttps ? 443 : 80)
        };

        Uri uri = uriBuilder.Uri;

        return Task.FromResult(uri);
    }

    public Task<string> GetTokenIssuerAsync()
    {
        string issuer = _httpContextAccessor.HttpContext!.Request.Host.Host;

        return Task.FromResult(issuer);
    }
}
