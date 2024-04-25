// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions;
using ExtDotNet.OAuth20.Server.Abstractions.Builders.Generic;
using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Models.Flows;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Endpoints.Authorization;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.1"/>
/// </summary>
public class DefaultAuthorizationEndpoint(
    IFlowRouter flowRouter,
    IArgumentsBuilder<FlowArguments> flowArgsBuilder,
    IRequestValidator<IAuthorizationEndpoint> requestValidator,
    IErrorResultProvider errorResultProvider,
    IClientService clientService,
    ILoginService loginService,
    IEndUserService endUserService,
    IScopeService scopeService,
    IPermissionsService permissionsService,
    IOptions<OAuth20ServerOptions> options) : IAuthorizationEndpoint
{
    private readonly IFlowRouter _flowRouter = flowRouter ?? throw new ArgumentNullException(nameof(flowRouter));
    private readonly IArgumentsBuilder<FlowArguments> _flowArgsBuilder = flowArgsBuilder ?? throw new ArgumentNullException(nameof(flowArgsBuilder));
    private readonly IRequestValidator<IAuthorizationEndpoint> _requestValidator = requestValidator ?? throw new ArgumentNullException(nameof(requestValidator));
    private readonly IErrorResultProvider _errorResultProvider = errorResultProvider ?? throw new ArgumentNullException(nameof(errorResultProvider));
    private readonly IClientService _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
    private readonly ILoginService _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
    private readonly IEndUserService _endUserService = endUserService ?? throw new ArgumentNullException(nameof(endUserService));
    private readonly IScopeService _scopeService = scopeService ?? throw new ArgumentNullException(nameof(scopeService));
    private readonly IPermissionsService _permissionsService = permissionsService ?? throw new ArgumentNullException(nameof(permissionsService));
    private readonly IOptions<OAuth20ServerOptions> _options = options ?? throw new ArgumentNullException(nameof(options));

    public async Task<IResult> InvokeAsync(HttpContext httpContext)
    {
        FlowArguments flowArgs = await _flowArgsBuilder.BuildArgumentsAsync(httpContext).ConfigureAwait(false);
        var validationResult = _requestValidator.TryValidate(httpContext);

        if (!flowArgs.Values.TryGetValue("state", out string? state) && _options.Value.AuthorizationRequestStateRequired)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(
                DefaultAuthorizeErrorType.InvalidRequest,
                state: null,
                "Missing request parameter: [state]");
        }

        if (!validationResult.Success)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.InvalidRequest, state, null);
        }

        if (!flowArgs.Values.TryGetValue("response_type", out string? responseType))
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.InvalidRequest, state, null);
        }

        if (!flowArgs.Values.TryGetValue("client_id", out string? clientId))
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.InvalidRequest, state, null);
        }

        Client? client = await _clientService.GetClientAsync(clientId).ConfigureAwait(false);
        if (client is null)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(
                DefaultAuthorizeErrorType.UnauthorizedClient,
                state,
                $"Client with [client_id] = [{clientId}] doesn't exist in the system.");
        }

        bool authenticated = await _endUserService.IsAuthenticatedAsync().ConfigureAwait(false);

        if (!authenticated) return await _loginService.RedirectToLoginAsync(flowArgs).ConfigureAwait(false);

        var endUser = await _endUserService.GetCurrentEndUserAsync().ConfigureAwait(false);
        if (endUser is null)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(
                DefaultAuthorizeErrorType.UnauthorizedClient,
                state,
                "Current EndUser doesn't exist in the system.");
        }

        flowArgs.Values.TryGetValue("scope", out string? clientRequestedScope);

        bool redirectToPermissionsRequired = await _permissionsService.RedirectToPermissionsRequiredAsync(endUser, client).ConfigureAwait(false);
        if (redirectToPermissionsRequired)
        {
            ScopeResult serverAllowedScopeResult = await _scopeService.GetServerAllowedScopeAsync(clientRequestedScope, client, state).ConfigureAwait(false);
            await _permissionsService.AddPermissionsRequestAsync(serverAllowedScopeResult, endUser, client).ConfigureAwait(false);

            return await _permissionsService.RedirectToPermissionsAsync(flowArgs, client, state).ConfigureAwait(false);
        }

        ScopeResult scopeResult;
        bool endUserPermissionsRequired = await _permissionsService.EndUserPermissionsRequiredAsync(client).ConfigureAwait(false);
        if (endUserPermissionsRequired)
        {
            scopeResult = await _scopeService.GetEndUserClientScopeAsync(clientRequestedScope, endUser, client, state).ConfigureAwait(false);
        }
        else
        {
            scopeResult = await _scopeService.GetServerAllowedScopeAsync(clientRequestedScope, client, state).ConfigureAwait(false);
        }

        if (_flowRouter.TryGetAuthorizeFlow(responseType, out IAuthorizeFlow? flow))
        {
            if (flow is null)
            {
                return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.ServerError, state, "Cannot determine the flow.");
            }

            return await flow.AuthorizeAsync(flowArgs, client, endUser, scopeResult).ConfigureAwait(false);
        }
        else
        {
            // TODO: handle only this node unsupported response type
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.UnsupportedResponseType, state, null);
        }
    }
}
