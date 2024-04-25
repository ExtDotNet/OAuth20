// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Models.Flows;
using ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Authorize;
using ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Token;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Flows;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.1"/>
/// </summary>
public class DefaultAuthorizationCodeFlow(
    IOptions<OAuth20ServerOptions> options,
    IErrorResultProvider errorResultProvider,
    IEndUserService endUserService,
    IClientService clientService,
    IScopeService scopeService,
    IAuthorizationCodeService authorizationCodeService,
    IRefreshTokenService refreshTokenService,
    IFlowService flowService,
    IPermissionsService permissionsService) : IAuthorizationCodeFlow
{
    private readonly IOptions<OAuth20ServerOptions> _options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly IErrorResultProvider _errorResultProvider = errorResultProvider ?? throw new ArgumentNullException(nameof(errorResultProvider));
    private readonly IEndUserService _endUserService = endUserService ?? throw new ArgumentNullException(nameof(endUserService));
    private readonly IClientService _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
    private readonly IScopeService _scopeService = scopeService ?? throw new ArgumentNullException(nameof(scopeService));
    private readonly IAuthorizationCodeService _authorizationCodeService = authorizationCodeService ?? throw new ArgumentNullException(nameof(authorizationCodeService));
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
    private readonly IFlowService _flowService = flowService ?? throw new ArgumentNullException(nameof(flowService));
    private readonly IPermissionsService _permissionsService = permissionsService ?? throw new ArgumentNullException(nameof(permissionsService));

    public async Task<IResult> AuthorizeAsync(FlowArguments args, Client client, EndUser endUser, ScopeResult scopeResult)
    {
        AuthorizeArguments authArgs = AuthorizeArguments.Create(args);
        if (authArgs.State is null && _options.Value.AuthorizationRequestStateRequired)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(
                DefaultAuthorizeErrorType.InvalidRequest,
                state: null,
                "Missing request parameter: [state]");
        }

        IResult result = await AuthorizeAsync(authArgs, endUser, client, scopeResult).ConfigureAwait(false);

        return result;
    }

    public async Task<IResult> AuthorizeAsync(AuthorizeArguments args, EndUser endUser, Client client, ScopeResult scopeResult)
    {
        var flow = await _flowService.GetFlowAsync<IAuthorizationCodeFlow>().ConfigureAwait(false);
        if (flow is null)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.ServerError, args.State, "Cannot determine the flow.");
        }

        bool flowAvailable = await _clientService.IsFlowAvailableForClientAsync(client, flow).ConfigureAwait(false);
        if (!flowAvailable)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.InvalidRequest, args.State, $"The selected flow is not available to the Client with [client_id] = [{args.ClientId}].");
        }

        string redirectUri = await _clientService.GetRedirectUriAsync(args.RedirectUri, flow, client, args.State).ConfigureAwait(false);

        string code = await _authorizationCodeService
            .GetAuthorizationCodeAsync(args, endUser, client, redirectUri, scopeResult.IssuedScope, scopeResult.IssuedScopeDifferent)
            .ConfigureAwait(false);
        if (code is null)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.ServerError, args.State, "Cannot issue a code.");
        }

        AuthorizeResult result = AuthorizeResult.Create(redirectUri, code, args.State);

        return result;
    }

    public async Task<IResult> GetTokenAsync(FlowArguments args, Client client)
    {
        var tokenArgs = TokenArguments.Create(args);

        IResult result = await GetTokenAsync(tokenArgs, client).ConfigureAwait(false);

        return result;
    }

    public async Task<IResult> GetTokenAsync(TokenArguments args, Client client)
    {
        var flow = await _flowService.GetFlowAsync<IAuthorizationCodeFlow>().ConfigureAwait(false);
        if (flow is null)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.ServerError, "Cannot determine the flow.");
        }

        AccessTokenResult accessToken = await _authorizationCodeService.ExchangeAuthorizationCodeAsync(args.Code, client, args.RedirectUri).ConfigureAwait(false);
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
