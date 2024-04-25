// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataStorages;
using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Models.Flows;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultPermissionsService(
    IOptions<OAuth20ServerOptions> options,
    IErrorResultProvider errorResultProvider,
    IServerMetadataService serverMetadataService,
    IScopeService scopeService,
    IEndUserClientScopeStorage endUserClientScopeStorage) : IPermissionsService
{
    private readonly IOptions<OAuth20ServerOptions> _options = options ?? throw new ArgumentNullException(nameof(options));
    private readonly IErrorResultProvider _errorResultProvider = errorResultProvider ?? throw new ArgumentNullException(nameof(errorResultProvider));
    private readonly IServerMetadataService _serverMetadataService = serverMetadataService ?? throw new ArgumentNullException(nameof(serverMetadataService));
    private readonly IScopeService _scopeService = scopeService ?? throw new ArgumentNullException(nameof(scopeService));
    private readonly IEndUserClientScopeStorage _endUserClientScopeStorage = endUserClientScopeStorage ?? throw new ArgumentNullException(nameof(endUserClientScopeStorage));

    public async Task<bool> RedirectToPermissionsRequiredAsync(EndUser endUser, Client client)
    {
        bool endUserPermissionsRequired = await EndUserPermissionsRequiredAsync(client).ConfigureAwait(false);

        if (endUserPermissionsRequired)
        {
            var hasEndUserClientScopeGranted = await _scopeService.HasEndUserClientScopeGrantedAsync(endUser, client).ConfigureAwait(false);

            if (!hasEndUserClientScopeGranted) return true;
        }

        return false;
    }

    public Task<bool> EndUserPermissionsRequiredAsync(Client client)
    {
        bool required = client.EndUserPermissionsRequired ?? _options.Value.EndUserPermissionsRequiredForClients;

        return Task.FromResult(required);
    }

    public async Task AddPermissionsRequestAsync(ScopeResult scopeResult, EndUser endUser, Client client)
    {
        EndUserClientScopeRequest request = new()
        {
            Username = endUser.Username,
            ClientId = client.ClientId,
            ClientRequestedScope = scopeResult.RequestedScope,
            ServerAllowedScopeDifferent = scopeResult.IssuedScopeDifferent,
            ServerAllowedScope = scopeResult.IssuedScope,
        };

        await _endUserClientScopeStorage.AddEndUserClientScopeRequestAsync(request).ConfigureAwait(false);
    }

    public async Task<ScopeResult?> GetPermissionsRequestAsync(EndUser endUser, Client client)
    {
        var request = await _endUserClientScopeStorage.GetEndUserClientScopeRequestAsync(endUser.Username, client.ClientId).ConfigureAwait(false);

        if (request == null) return null;

        ScopeResult scopeResult = new()
        {
            RequestedScope = request.ClientRequestedScope,
            IssuedScopeDifferent = request.ServerAllowedScopeDifferent,
            IssuedScope = request.ServerAllowedScope,
        };

        return scopeResult;
    }

    public async Task AddPermissionsResultAsync(ScopeResult scopeResult, EndUser endUser, Client client)
    {
        EndUserClientScopeResult request = new()
        {
            Username = endUser.Username,
            ClientId = client.ClientId,
            ServerAllowedScope = scopeResult.RequestedScope!,
            EndUserIssuedScopeDifferent = scopeResult.IssuedScopeDifferent,
            EndUserIssuedScope = scopeResult.IssuedScope,
        };

        await _endUserClientScopeStorage.AddEndUserClientScopeResultAsync(request).ConfigureAwait(false);
    }

    public async Task<ScopeResult?> GetPermissionsResultAsync(EndUser endUser, Client client)
    {
        var result = await _endUserClientScopeStorage.GetEndUserClientScopeResultAsync(endUser.Username, client.ClientId).ConfigureAwait(false);

        if (result == null) return null;

        ScopeResult scopeResult = new()
        {
            RequestedScope = result.ServerAllowedScope,
            IssuedScopeDifferent = result.EndUserIssuedScopeDifferent,
            IssuedScope = result.EndUserIssuedScope,
        };

        return scopeResult;
    }

    public async Task<IResult> RedirectToPermissionsAsync(FlowArguments flowArgs, Client client, string? state = null)
    {
        string? permissionsEndpoint = client.PermissionsEndpoint ?? _options.Value.DefaultPermissionsEndpoint;
        if (permissionsEndpoint is null)
        {
            return _errorResultProvider.GetAuthorizeErrorResult(DefaultAuthorizeErrorType.ServerError, state: state, "Permissions endpoint isn't registered.");
        }

        Uri permissionsEndpointUri = new(permissionsEndpoint, UriKind.RelativeOrAbsolute);
        if (!permissionsEndpointUri.IsAbsoluteUri)
        {
            Uri instanceUri = await _serverMetadataService.GetCurrentInstanceUriAsync().ConfigureAwait(false);
            permissionsEndpointUri = new Uri(instanceUri, permissionsEndpointUri);
        }

        PermissionsRedirectResult result = new(permissionsEndpointUri.ToString(), flowArgs);

        return result;
    }
}
