// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IServerUriService
{
    public Task<string?> GetServerRelativeUriPrefix();

    public Task<Uri> GetServerAbsoluteUri(Uri relativeUri);

    public Task<Uri> GetServerAbsoluteUri(string relativeUri);

    public Task<Uri> GetServerRelativeUri(Uri relativeUri);

    public Task<Uri> GetServerRelativeUri(string relativeUri);
}
