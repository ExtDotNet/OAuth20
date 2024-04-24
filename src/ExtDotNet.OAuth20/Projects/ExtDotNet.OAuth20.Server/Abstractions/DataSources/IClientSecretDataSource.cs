// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.DataSources;

public interface IClientSecretDataSource
{
    public Task<ClientSecret?> GetClientSecretAsync(string type, string clientSecretContent);

    public Task<ClientSecret?> GetEmptyClientSecretAsync(string type, Client client);
}
