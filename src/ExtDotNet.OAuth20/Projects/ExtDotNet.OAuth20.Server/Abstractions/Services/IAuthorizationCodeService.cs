// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Authorize;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IAuthorizationCodeService
{
    public Task<string> GetAuthorizationCodeAsync(AuthorizeArguments args, EndUser endUser, Client client, string redirectUri, string issuedScope, bool issuedScopeDifferent);

    public Task<AccessTokenResult> ExchangeAuthorizationCodeAsync(string code, Client client, string? redirectUri);
}
