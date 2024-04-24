// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IEndUserService
{
    public Task<EndUser?> GetCurrentEndUserAsync(string? state = null);

    public Task<EndUser?> GetEndUserAsync(string username);

    public Task<EndUser?> GetEndUserAsync(string username, string password);

    public bool IsAuthenticated();
}
