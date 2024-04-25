// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;
using ExtDotNet.OAuth20.Server.Abstractions.DataStorages;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;
using ExtDotNet.OAuth20.Server.Abstractions.Interceptors;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultScopeService : IScopeService
{
    private readonly IScopeDataSource _scopeDataSource;
    private readonly IEndUserClientScopeStorage _endUserClientScopeStorage;
    private readonly IOptions<OAuth20ServerOptions> _options;
    private readonly IScopeInterceptor? _scopeInterceptor;

    public DefaultScopeService(
        IScopeDataSource scopeDataSource,
        IEndUserClientScopeStorage endUserClientScopeStorage,
        IOptions<OAuth20ServerOptions> options,
        IScopeInterceptor? scopeInterceptor = null)
    {
        _scopeDataSource = scopeDataSource;
        _endUserClientScopeStorage = endUserClientScopeStorage;
        _options = options;
        _scopeInterceptor = scopeInterceptor;
    }

    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.3"/>
    /// - 01. Scope request parameter is allowed to specify prefered scope. // Used by the following endpoints: authorize, token.
    /// - 02. Scope response parameter is used to inform about issued scope. // Used by the following endpoints: token.
    /// - 03. The authorization server SHOULD document its scope requirements and default value (if defined). // Used by the authorization server's documentation.
    /// - 1. auth server may fully or partially ignore the scope parameter requested by the client // Used by the following services: scope service.
    /// (based on the auth server policy or enduser restrictions) // Used by the following services: scope service.
    /// - 2. If the issued access token scope is different from the one requested by the client, // Used by the following services: scope service.
    /// the authorization server MUST include the "scope" response parameter to inform the client of the actual scope granted. // Used by the following endpoints: token.
    /// - 3. If the client omits the scope parameter when requesting authorization, // Used by the following services: scope service.
    /// the authorization server MUST either process the request using a pre-defined default value or // Used by the following endpoints: token.
    /// fail the request indicating an invalid scope. // Used by the following endpoints: authorize.
    /// </summary>
    public async Task<ScopeResult> GetServerAllowedScopeAsync(string? clientRequestedScope, Client client, string? state = null)
    {
        // Here is the possibility of executing an user-defined interception of the requested scope.
        if (_scopeInterceptor is not null)
        {
            clientRequestedScope = await _scopeInterceptor.OnExecutingAsync(clientRequestedScope, client, state).ConfigureAwait(false);
        }

        if (clientRequestedScope is null && _options.Value.AuthorizationRequestScopeRequired)
        {
            throw new InvalidScopeException("Missing request parameter: [scope]", state);
        }

        IEnumerable<Scope> allowedScopes = await _scopeDataSource.GetScopesAsync(client).ConfigureAwait(false);

        if (_options.Value.UserScopeAllowanceRequired && !allowedScopes.Any())
        {
            // TODO: correct the messsage
            throw new InvalidScopeException(
                $"There isn't any scope is allowed by the current EndUser for the Client with [client_id] = [{client.ClientId}]." +
                "Before the issuing a scope the server requires every Client to obtain from EndUser a single allowed scope at least.",
                    state);
        }

        if (clientRequestedScope is null)
        {
            // Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.3"/>
            // If the client omits the scope parameter when requesting authorization,
            // The authorization server MUST either process the request using a pre-defined default value
            // or fail the request indicating an invalid scope.
            // * The authorization server SHOULD document its scope requirements and default value (if defined).
            // All information should be provided by a <see cref="IServerInformationService"/> instance.
            if (_options.Value.ScopePreDefinedDefaultValue?.Any() is not true)
            {
                throw new InvalidScopeException(
                    $"There isn't any scope default value was pre-defined by the server but the Client with [client_id] = [{client.ClientId}]" +
                    "made a request with no specifying scope.",
                    state);
            }

            List<Scope> loadedScopeModels = new(_options.Value.ScopePreDefinedDefaultValue.Count());

            foreach (string preDefinedScopeName in _options.Value.ScopePreDefinedDefaultValue)
            {
                Scope? preDefinedScopeModel = await _scopeDataSource.GetScopeAsync(preDefinedScopeName).ConfigureAwait(false);

                if (preDefinedScopeModel is null)
                {
                    throw new InvalidScopeException(
                        $"The server specified the scope with [name] = [{preDefinedScopeName}] as pre-defined scope value " +
                        "but there isn't any scope with this name in the system.",
                        state);
                }

                loadedScopeModels.Add(preDefinedScopeModel);
            }

            IEnumerable<Scope> issuedScopeModels = loadedScopeModels;

            // Here is the possibility of executing an user-defined interception of the issued scope models.
            if (_scopeInterceptor is not null)
            {
                issuedScopeModels = await _scopeInterceptor.OnExecutedAsync(issuedScopeModels, client, state).ConfigureAwait(false);
            }

            string issuedScopeValue = issuedScopeModels.Select(x => x.Name).Aggregate((first, second) => $"{first} {second}");

            // Here is the possibility of executing an user-defined interception of the issued scope string.
            if (_scopeInterceptor is not null)
            {
                issuedScopeValue = await _scopeInterceptor.OnExecutedAsync(issuedScopeValue, client, state).ConfigureAwait(false);
            }

            ScopeResult issuedScope = new()
            {
                RequestedScope = null,
                IssuedScopeDifferent = false,
                IssuedScope = issuedScopeValue
            };

            return issuedScope;
        }
        else
        {
            IEnumerable<string> requestedScopeNames = clientRequestedScope.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            List<Scope> loadedScopeModels = new(requestedScopeNames.Count());

            foreach (string requestedScopeName in requestedScopeNames)
            {
                Scope? requestedScopeModel = await _scopeDataSource.GetScopeAsync(requestedScopeName).ConfigureAwait(false);

                if (requestedScopeModel is null)
                {
                    throw new InvalidScopeException(
                        $"Client with [client_id] = [{client.ClientId}] made a request with specifying a scope with [name] = [{requestedScopeName}] " +
                        "but there isn't any scope with this name is allowed for this client in the system.",
                        state);
                }

                loadedScopeModels.Add(requestedScopeModel);
            }

            IEnumerable<Scope> requestedScopeModels = loadedScopeModels;

            // Here is the possibility of executing an user-defined interception of the requested scope.
            if (_scopeInterceptor is not null)
            {
                requestedScopeModels = await _scopeInterceptor.OnExecutingAsync(requestedScopeModels, client, state).ConfigureAwait(false);
            }

            IEnumerable<Scope> issuedScopeModels = requestedScopeModels.IntersectBy(allowedScopes.Select(x => x.Name), x => x.Name).ToList();

            // Here is the possibility of executing an user-defined interception of the issued scope models.
            if (_scopeInterceptor is not null)
            {
                issuedScopeModels = await _scopeInterceptor.OnExecutedAsync(issuedScopeModels, client, state).ConfigureAwait(false);
            }

            string issuedScopeValue = issuedScopeModels.Select(x => x.Name).Aggregate((first, second) => $"{first} {second}");

            // Here is the possibility of executing an user-defined interception of the issued scope string.
            if (_scopeInterceptor is not null)
            {
                issuedScopeValue = await _scopeInterceptor.OnExecutedAsync(issuedScopeValue, client, state).ConfigureAwait(false);
            }

            // Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.3"/>
            // If the issued access token scope is different from the one requested by the client, the authorization
            // server MUST include the "scope" response parameter to inform the client of the actual scope granted.
            bool issuedScopeDifferent =
                _options.Value.InclusionScopeToResponseRequired ||
                issuedScopeModels.Count() != loadedScopeModels.Count();

            ScopeResult issuedScope = new()
            {
                RequestedScope = clientRequestedScope,
                IssuedScopeDifferent = issuedScopeDifferent,
                IssuedScope = issuedScopeValue
            };

            return issuedScope;
        }
    }

    public async Task<IEnumerable<Scope>> GetScopeListAsync(string scope)
    {
        string[] names = scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return await _scopeDataSource.GetScopesAsync(names).ConfigureAwait(false);
    }

    public async Task<bool> HasEndUserClientScopeGrantedAsync(EndUser endUser, Client client)
    {
        var endUserClientScopeResult = await _endUserClientScopeStorage.GetEndUserClientScopeResultAsync(endUser.Username, client.ClientId).ConfigureAwait(false);

        bool hasGranted = endUserClientScopeResult is not null;

        return hasGranted;
    }

    public async Task<ScopeResult> GetEndUserClientScopeAsync(string? requestedScope, EndUser endUser, Client client, string? state = null)
    {
        var endUserClientScopeResult = await _endUserClientScopeStorage.GetEndUserClientScopeResultAsync(endUser.Username, client.ClientId).ConfigureAwait(false);
        // TODO: detail the error message
        if (endUserClientScopeResult is null) throw new Exception();

        string issuedScope = endUserClientScopeResult.EndUserIssuedScope;

        string[] scopes = issuedScope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        bool allScopesValid = await _scopeDataSource.AllScopesValidAsync(scopes).ConfigureAwait(false);
        // TODO: detail the error message
        if (!allScopesValid) throw new Exception();

        ScopeResult scopeResult = new()
        {
            RequestedScope = requestedScope,
            IssuedScope = issuedScope,
            IssuedScopeDifferent = endUserClientScopeResult.EndUserIssuedScopeDifferent
        };

        return scopeResult;
    }

    public bool ScopesEqual(string scope1, string? scope2)
    {
        if (scope2 is null) return false;

        if (scope1 == scope2) return true;

        IEnumerable<string> scope1List = scope1.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        IEnumerable<string> scope2List = scope2.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        bool equal = scope1List.OrderBy(x => x).SequenceEqual(scope2List.OrderBy(x => x));

        return equal;
    }
}
