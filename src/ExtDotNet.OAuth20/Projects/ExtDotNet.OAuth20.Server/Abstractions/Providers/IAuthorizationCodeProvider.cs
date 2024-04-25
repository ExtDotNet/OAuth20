// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Authorize;

namespace ExtDotNet.OAuth20.Server.Abstractions.Providers;

public interface IAuthorizationCodeProvider
{
    public ValueTask<string> GetAuthorizationCodeValueAsync(AuthorizeArguments args, EndUser endUser, Client client, string redirectUri, string scope);
}
