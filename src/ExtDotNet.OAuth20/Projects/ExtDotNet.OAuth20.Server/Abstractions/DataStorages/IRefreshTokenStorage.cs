// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.Abstractions.DataStorages;

public interface IRefreshTokenStorage
{
    public Task AddRefreshTokenAsync(RefreshTokenResult token);

    public Task<RefreshTokenResult?> GetRefreshTokenAsync(string token);
}
