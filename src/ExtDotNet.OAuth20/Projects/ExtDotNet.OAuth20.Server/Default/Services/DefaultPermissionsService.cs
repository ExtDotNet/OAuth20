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

public class DefaultPermissionsService : IPermissionsService
{
    private readonly IOptions<OAuth20ServerOptions> _options;
    private readonly IErrorResultProvider _errorResultProvider;
    private readonly IServerMetadataService _serverMetadataService;
    private readonly IScopeService _scopeService;
    private readonly IEndUserClientScopeStorage _endUserClientScopeStorage;

    public DefaultPermissionsService(
        IOptions<OAuth20ServerOptions> options,
        IErrorResultProvider errorResultProvider,
        IServerMetadataService serverMetadataService,
        IScopeService scopeService,
        IEndUserClientScopeStorage endUserClientScopeStorage)
    {
        _options = options;
        _errorResultProvider = errorResultProvider;
        _serverMetadataService = serverMetadataService;
        _scopeService = scopeService;
        _endUserClientScopeStorage = endUserClientScopeStorage;
    }

    public async Task<bool> RedirectToPermissionsRequiredAsync(EndUser endUser, Client client)
    {
        bool endUserPermissionsRequired = await EndUserPermissionsRequiredAsync(client);

        if (endUserPermissionsRequired)
        {
            var hasEndUserClientScopeGranted = await _scopeService.HasEndUserClientScopeGrantedAsync(endUser, client);

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

        await _endUserClientScopeStorage.AddEndUserClientScopeRequestAsync(request);
    }

    public async Task<ScopeResult?> GetPermissionsRequestAsync(EndUser endUser, Client client)
    {
        var request = await _endUserClientScopeStorage.GetEndUserClientScopeRequestAsync(endUser.Username, client.ClientId);

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

        await _endUserClientScopeStorage.AddEndUserClientScopeResultAsync(request);
    }

    public async Task<ScopeResult?> GetPermissionsResultAsync(EndUser endUser, Client client)
    {
        var result = await _endUserClientScopeStorage.GetEndUserClientScopeResultAsync(endUser.Username, client.ClientId);

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
            Uri instanceUri = await _serverMetadataService.GetCurrentInstanceUriAsync();
            permissionsEndpointUri = new Uri(instanceUri, permissionsEndpointUri);
        }

        PermissionsRedirectResult result = new(permissionsEndpointUri.ToString(), flowArgs);

        return result;
    }
}
