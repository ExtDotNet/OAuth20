// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IAccessTokenService
{
    public Task<AccessTokenResult> GetAccessTokenAsync(string issuedScope, bool issuedScopeDifferent, Client client, string? redirectUri = null, EndUser? endUser = null);
}
