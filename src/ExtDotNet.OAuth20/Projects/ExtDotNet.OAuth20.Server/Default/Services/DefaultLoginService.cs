// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Models.Flows;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultLoginService : ILoginService
{
    private readonly IOptions<OAuth20ServerOptions> _options;
    private readonly IClientService _clientService;
    private readonly IErrorResultProvider _errorResultProvider;
    private readonly IServerMetadataService _serverMetadataService;

    public DefaultLoginService(
        IOptions<OAuth20ServerOptions> options,
        IClientService clientService,
        IErrorResultProvider errorResultProvider,
        IServerMetadataService serverMetadataService)
    {
        _options = options;
        _clientService = clientService;
        _errorResultProvider = errorResultProvider;
        _serverMetadataService = serverMetadataService;
    }

    public async Task<IResult> RedirectToLoginAsync(FlowArguments args)
    {
        if (!args.Values.TryGetValue("state", out string? state) && _options.Value.AuthorizationRequestStateRequired)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.InvalidRequest, state: null, "Missing request parameter: [state]");
        }

        if (!args.Values.TryGetValue("client_id", out string? clientId))
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.InvalidRequest, state: state, "Missing request parameter: [client_id]");
        }

        var client = await _clientService.GetClientAsync(clientId).ConfigureAwait(false);
        if (client is null)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.UnauthorizedClient, state: state, $"Client with [client_id] = [{clientId}] doesn't exist.");
        }

        string? loginEndpoint = client.LoginEndpoint ?? _options.Value.DefaultLoginEndpoint;
        if (loginEndpoint is null)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.ServerError, state: state, "Login endpoint isn't registered.");
        }

        Uri loginEndpointUri = new(loginEndpoint, UriKind.RelativeOrAbsolute);
        if (!loginEndpointUri.IsAbsoluteUri)
        {
            Uri instanceUri = await _serverMetadataService.GetCurrentInstanceUriAsync().ConfigureAwait(false);
            loginEndpointUri = new Uri(instanceUri, loginEndpointUri);
        }

        LoginRedirectResult result = new(loginEndpointUri.ToString(), args);

        return result;
    }
}
