// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;
using ExtDotNet.OAuth20.Server.Abstractions.Providers;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.Default.Providers;

public class DefaultTokenProvider(
    ITokenBuilderSelector tokenBuilderSelector,
    IClientService clientService) : ITokenProvider
{
    private readonly ITokenBuilderSelector _tokenBuilderSelector = tokenBuilderSelector ??
            throw new ArgumentNullException(nameof(tokenBuilderSelector));

    private readonly IClientService _clientService = clientService ??
            throw new ArgumentNullException(nameof(clientService));

    public async Task<TokenType> GetTokenTypeAsync(Client client) =>
        await _clientService.GetTokenType(client).ConfigureAwait(false);

    public async Task<string> GetTokenValueAsync(TokenType tokenType, TokenContext tokenContext)
    {
        if (!_tokenBuilderSelector.TryGetTokenBuilder(tokenType.Name, out ITokenBuilder? tokenBuilder))
        {
            throw new ServerConfigurationErrorException(
                $"There are no registered token builder for the specified token type " +
                $"[{tokenType.Name}] in this server instance.");
        }

        return await tokenBuilder!.BuildTokenAsync(tokenContext).ConfigureAwait(false);
    }
}
