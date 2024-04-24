// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.Abstractions.DataStorages;

public interface IAuthorizationCodeStorage
{
    public Task AddAuthorizationCodeResultAsync(AuthorizationCodeResult code);

    public Task<AuthorizationCodeResult?> GetAuthorizationCodeResultAsync(string code);
}
