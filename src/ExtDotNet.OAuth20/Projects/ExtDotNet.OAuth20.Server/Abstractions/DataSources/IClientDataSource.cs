// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.DataSources;

public interface IClientDataSource
{
    public Task<Client?> GetClientAsync(string clientId);

    public Task<Client> GetClientAsync(ClientSecret clientSecret);

    public Task<IEnumerable<Flow>> GetClientFlowsAsync(string clientId);

    public Task<IEnumerable<ClientRedirectionEndpoint>> GetClientRedirectionEndpointsAsync(string clientId);
}
