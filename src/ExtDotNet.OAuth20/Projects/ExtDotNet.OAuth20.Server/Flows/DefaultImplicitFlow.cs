// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Models.Flows;
using ExtDotNet.OAuth20.Server.Models.Flows.Implicit;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Flows;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-4.2"/>
/// </summary>
public class DefaultImplicitFlow(
    IOptions<OAuth20ServerOptions> options,
    IErrorResultProvider errorResultProvider,
    IEndUserService endUserService,
    ILoginService loginService,
    IClientService clientService,
    IFlowService flowService,
    IScopeService scopeService,
    IAccessTokenService accessTokenService,
    IPermissionsService permissionsService) : IImplicitFlow
{
    private readonly IOptions<OAuth20ServerOptions> _options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly IErrorResultProvider _errorResultProvider = errorResultProvider ?? throw new ArgumentNullException(nameof(errorResultProvider));
    private readonly IEndUserService _endUserService = endUserService ?? throw new ArgumentNullException(nameof(endUserService));
    private readonly ILoginService _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
    private readonly IClientService _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
    private readonly IFlowService _flowService = flowService ?? throw new ArgumentNullException(nameof(flowService));
    private readonly IScopeService _scopeService = scopeService ?? throw new ArgumentNullException(nameof(scopeService));
    private readonly IAccessTokenService _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
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
        var flow = await _flowService.GetFlowAsync<IImplicitFlow>().ConfigureAwait(false);
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

        AccessTokenResult accessToken = await _accessTokenService.GetAccessTokenAsync(
                scopeResult.IssuedScope,
                scopeResult.IssuedScopeDifferent,
                client,
                redirectUri,
                endUser)
            .ConfigureAwait(false);

        TokenResult result = TokenResult.Create(
            redirectUri: redirectUri,
            accessToken: accessToken.Value,
            tokenType: accessToken.Type,
            expiresInRequired: _options.Value.TokenResponseExpiresInRequired,
            expiresIn: accessToken.ExpiresIn,
            scope: accessToken.IssuedScopeDifferent ? accessToken.Scope : null,
            null, // TODO: figure additional parameters out
            args.State);

        return result;
    }
}
