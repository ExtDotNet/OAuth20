// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Flows.ClientCredentials.Token;

namespace ExtDotNet.OAuth20.Server.Flows;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.4"/>
/// </summary>
public interface IClientCredentialsFlow : ITokenFlow
{
    Task<IResult> GetTokenAsync(TokenArguments args, Client client);
}
