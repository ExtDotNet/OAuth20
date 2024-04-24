// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.DataSources;

public interface IEndUserDataSource
{
    public Task<EndUser?> GetEndUserAsync(string username);

    public Task<EndUser?> GetEndUserAsync(string username, string? passwordHash);
}
