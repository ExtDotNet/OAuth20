// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;

using ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Authorize;
using ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Token;

namespace ExtDotNet.OAuth20.Server.Abstractions.Flows;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.1"/>
/// </summary>
public interface IAuthorizationCodeFlow : IAuthorizeFlow, ITokenFlow
{
    Task<IResult> AuthorizeAsync(AuthorizeArguments args, EndUser endUser, Client client, ScopeResult scopeResult);

    Task<IResult> GetTokenAsync(TokenArguments args, Client client);
}
