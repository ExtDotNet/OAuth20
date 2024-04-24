// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultServerUriService : IServerUriService
{
    private readonly IServerMetadataService _serverMetadataService;
    private readonly IOptions<OAuth20ServerOptions> _options;

    public DefaultServerUriService(
        IServerMetadataService serverMetadataService,
        IOptions<OAuth20ServerOptions> options)
    {
        _serverMetadataService = serverMetadataService;
        _options = options;
    }

    public async Task<Uri> GetServerAbsoluteUri(Uri relativeUri)
    {
        string path = relativeUri.LocalPath;

        return await GetServerAbsoluteUri(path);
    }

    public async Task<Uri> GetServerAbsoluteUri(string relativeUri)
    {
        Uri uri = await _serverMetadataService.GetCurrentInstanceUriAsync();
        Uri completeRelativeUri = await GetServerRelativeUri(relativeUri);

        UriBuilder builder = new()
        {
            Scheme = uri.Scheme,
            Host = uri.Host,
            Port = uri.Port,
            Path = completeRelativeUri.LocalPath
        };

        return builder.Uri;
    }

    public Task<string?> GetServerRelativeUriPrefix()
    {
        return Task.FromResult(_options.Value.AuthorizationServerRelativeUriPrefix);
    }

    public async Task<Uri> GetServerRelativeUri(Uri relativeUri)
    {
        string? prefix = await GetServerRelativeUriPrefix();

        string path;

        if (prefix is not null)
        {
            string formattedRelativeUri =
                relativeUri.LocalPath.StartsWith('/') ? relativeUri.LocalPath : "/" + relativeUri.LocalPath;

            path = prefix + formattedRelativeUri;
        }
        else
        {
            path = relativeUri.LocalPath;
        }

        UriBuilder builder = new()
        {
            Path = path
        };

        return builder.Uri;
    }

    public Task<Uri> GetServerRelativeUri(string relativeUri)
    {
        UriBuilder builder = new()
        {
            Path = relativeUri
        };

        return Task.FromResult(builder.Uri);
    }
}
