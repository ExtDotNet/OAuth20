// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IPasswordHashingService
{
    public Task<string?> GetPasswordHashAsync(string? password);
}
