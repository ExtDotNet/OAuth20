// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Flows.RefreshToken;

namespace ExtDotNet.OAuth20.Server.Flows;

/// <summary>
/// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-1.5
/// </summary>
public interface IRefreshTokenFlow : ITokenFlow
{
    Task<TokenResult> GetTokenAsync(TokenArguments args, Client client);
}
