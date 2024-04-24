// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.Abstractions.DataStorages;

public interface IEndUserClientScopeStorage
{
    public Task AddEndUserClientScopeRequestAsync(EndUserClientScopeRequest endUserClientScopeRequest);

    public Task<EndUserClientScopeRequest?> GetEndUserClientScopeRequestAsync(string username, string clientId);

    public Task AddEndUserClientScopeResultAsync(EndUserClientScopeResult endUserClientScopeResult);

    public Task<EndUserClientScopeResult?> GetEndUserClientScopeResultAsync(string username, string clientId);
}
