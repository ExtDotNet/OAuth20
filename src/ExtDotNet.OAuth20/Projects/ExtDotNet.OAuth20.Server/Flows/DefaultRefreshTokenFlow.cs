// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Flows;
using ExtDotNet.OAuth20.Server.Models.Flows.RefreshToken;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Flows;

/// <summary>
/// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-1.5
/// </summary>
public class DefaultRefreshTokenFlow : IRefreshTokenFlow
{
    private readonly IOptions<OAuth20ServerOptions> _options;
    private readonly IErrorResultProvider _errorResultProvider;

    public DefaultRefreshTokenFlow(IOptions<OAuth20ServerOptions> options, IErrorResultProvider errorResultProvider)
    {
        _options = options;
        _errorResultProvider = errorResultProvider;
    }

    public async Task<IResult> GetTokenAsync(FlowArguments args, Client client)
    {
        TokenArguments tokenArgs = TokenArguments.Create(args);

        var result = await GetTokenAsync(tokenArgs, client).ConfigureAwait(false);

        return result;
    }

    public Task<TokenResult> GetTokenAsync(TokenArguments args, Client client)
    {
        throw new NotImplementedException();
    }
}
