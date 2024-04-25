// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions;
using ExtDotNet.OAuth20.Server.Abstractions.Builders.Generic;
using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Flows;

namespace ExtDotNet.OAuth20.Server.Endpoints.Token;

/// <summary>
/// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.2"/>
/// </summary>
public class DefaultTokenEndpoint : ITokenEndpoint
{
    private readonly IFlowRouter _flowRouter;
    private readonly IArgumentsBuilder<FlowArguments> _flowArgsBuilder;
    private readonly IRequestValidator<ITokenEndpoint> _requestValidator;
    private readonly IErrorResultProvider _errorResultProvider;
    private readonly IClientAuthenticationService _clientAuthenticationService;

    public DefaultTokenEndpoint(
        IFlowRouter flowRouter,
        IArgumentsBuilder<FlowArguments> flowArgsBuilder,
        IRequestValidator<ITokenEndpoint> requestValidator,
        IErrorResultProvider errorResultProvider,
        IClientAuthenticationService clientAuthenticationService)
    {
        _flowRouter = flowRouter;
        _flowArgsBuilder = flowArgsBuilder;
        _requestValidator = requestValidator;
        _errorResultProvider = errorResultProvider;
        _clientAuthenticationService = clientAuthenticationService;
    }

    public async Task<IResult> InvokeAsync(HttpContext httpContext)
    {
        var validationResult = _requestValidator.TryValidate(httpContext);

        FlowArguments flowArgs = await _flowArgsBuilder.BuildArgumentsAsync(httpContext).ConfigureAwait(false);

        if (!validationResult.Success)
        {
            flowArgs.Values.TryGetValue("state", out string? state);
            return _errorResultProvider.GetTokenErrorResult(DefaultTokenErrorType.InvalidRequest, state, null);
        }

        if (!flowArgs.Values.TryGetValue("grant_type", out string? responseType))
        {
            flowArgs.Values.TryGetValue("state", out string? state);
            return _errorResultProvider.GetTokenErrorResult(DefaultTokenErrorType.InvalidRequest, state, null);
        }

        Client? client = await _clientAuthenticationService.AuthenticateClientAsync(httpContext).ConfigureAwait(false);
        if (client is null)
        {
            flowArgs.Values.TryGetValue("state", out string? state);
            return _errorResultProvider.GetTokenErrorResult(DefaultTokenErrorType.InvalidClient, state, null);
        }

        if (_flowRouter.TryGetTokenFlow(responseType, out ITokenFlow? flow))
        {
            return await flow!.GetTokenAsync(flowArgs, client).ConfigureAwait(false);
        }
        else
        {
            flowArgs.Values.TryGetValue("state", out string? state);
            return _errorResultProvider.GetTokenErrorResult(DefaultTokenErrorType.UnsupportedGrantType, state, null);
        }
    }
}
