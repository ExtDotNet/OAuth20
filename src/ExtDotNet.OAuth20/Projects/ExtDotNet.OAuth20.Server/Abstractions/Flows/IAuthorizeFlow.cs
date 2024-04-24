// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;

using ExtDotNet.OAuth20.Server.Models.Flows;

namespace ExtDotNet.OAuth20.Server.Abstractions.Flows;

/// <summary>
/// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-1.3
/// </summary>
public interface IAuthorizeFlow : IFlow
{
    public Task<IResult> AuthorizeAsync(FlowArguments args, Client client, EndUser endUser, ScopeResult scopeResult);
}
