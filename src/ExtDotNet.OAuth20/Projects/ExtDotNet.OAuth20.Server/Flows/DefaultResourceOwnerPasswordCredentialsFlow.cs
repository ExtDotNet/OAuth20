// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Models.Flows;
using ExtDotNet.OAuth20.Server.Models.Flows.ResourceOwnerPasswordCredentials;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Flows;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.3"/>
/// </summary>
public class DefaultResourceOwnerPasswordCredentialsFlow(
    IOptions<OAuth20ServerOptions> options,
    IErrorResultProvider errorResultProvider,
    IEndUserService endUserService,
    IFlowService flowService,
    IClientService clientService,
    IScopeService scopeService,
    IAccessTokenService accessTokenService,
    IRefreshTokenService refreshTokenService) : IResourceOwnerPasswordCredentialsFlow
{
    private readonly IOptions<OAuth20ServerOptions> _options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly IErrorResultProvider _errorResultProvider = errorResultProvider ?? throw new ArgumentNullException(nameof(errorResultProvider));
    private readonly IEndUserService _endUserService = endUserService ?? throw new ArgumentNullException(nameof(endUserService));
    private readonly IFlowService _flowService = flowService ?? throw new ArgumentNullException(nameof(flowService));
    private readonly IClientService _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
    private readonly IScopeService _scopeService = scopeService ?? throw new ArgumentNullException(nameof(scopeService));
    private readonly IAccessTokenService _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));

    public async Task<IResult> GetTokenAsync(FlowArguments args, Client client)
    {
        var tokenArgs = TokenArguments.Create(args);

        var result = await GetTokenAsync(tokenArgs, client).ConfigureAwait(false);
        return result;
    }

    public async Task<IResult> GetTokenAsync(TokenArguments args, Client client)
    {
        var endUser = await _endUserService.GetEndUserAsync(args.Username, args.Password).ConfigureAwait(false);
        if (endUser is null)
        {
            // TODO: token server error or something
            return _errorResultProvider.GetTokenErrorResult(DefaultTokenErrorType.UnsupportedGrantType, null, "EndUser's authentication failed. Incorrect username or password.");
        }

        var flow = await _flowService.GetFlowAsync<IResourceOwnerPasswordCredentialsFlow>().ConfigureAwait(false);
        if (flow is null)
        {
            // TODO: token server error or something
            return _errorResultProvider.GetTokenErrorResult(DefaultTokenErrorType.Undefined, null, "Cannot determine the flow.");
        }

        bool flowAvailable = await _clientService.IsFlowAvailableForClientAsync(client, flow).ConfigureAwait(false);
        if (!flowAvailable)
        {
            return _errorResultProvider.GetTokenErrorResult(DefaultTokenErrorType.InvalidRequest, null, $"The selected flow is not available to the Client with [client_id] = [{client.ClientId}].");
        }

        ScopeResult scopeResult = await _scopeService.GetServerAllowedScopeAsync(args.Scope, client).ConfigureAwait(false);

        AccessTokenResult accessToken = await _accessTokenService
            .GetAccessTokenAsync(
                scopeResult.IssuedScope,
                scopeResult.IssuedScopeDifferent,
                client,
                null,
                endUser)
            .ConfigureAwait(false);

        RefreshTokenResult refreshToken = await _refreshTokenService.GetRefreshTokenAsync(accessToken).ConfigureAwait(false);

        TokenResult result = TokenResult.Create(
            accessToken: accessToken.Value,
            tokenType: accessToken.Type,
            expiresInRequired: _options.Value.TokenResponseExpiresInRequired,
            expiresIn: accessToken.ExpiresIn,
            scope: accessToken.IssuedScopeDifferent ? accessToken.Scope : null,
            null, // TODO: figure additional parameters out
            refreshToken.Value);

        return result;
    }
}
