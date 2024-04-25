// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;
using ExtDotNet.OAuth20.Server.Abstractions.DataStorages;
using ExtDotNet.OAuth20.Server.Abstractions.Providers;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultAccessTokenService : IAccessTokenService
{
    private readonly IAccessTokenStorage _tokenStorage;
    private readonly IServerMetadataService _serverMetadataService;
    private readonly IDateTimeService _dateTimeService;
    private readonly ITokenTypeDataSource _tokenTypeDataSource;
    private readonly ITokenProvider _tokenProvider;
    private readonly IScopeService _scopeService;
    private readonly IResourceService _resourceService;
    private readonly IOptions<OAuth20ServerOptions> _options;

    public DefaultAccessTokenService(
        IAccessTokenStorage tokenStorage,
        IServerMetadataService serverMetadataService,
        IDateTimeService dateTimeService,
        ITokenTypeDataSource tokenTypeDataSource,
        ITokenProvider tokenProvider,
        IScopeService scopeService,
        IResourceService resourceService,
        IOptions<OAuth20ServerOptions> options)
    {
        _tokenStorage = tokenStorage;
        _serverMetadataService = serverMetadataService;
        _dateTimeService = dateTimeService;
        _tokenTypeDataSource = tokenTypeDataSource;
        _tokenProvider = tokenProvider;
        _scopeService = scopeService;
        _resourceService = resourceService;
        _options = options;
    }

    public async Task<AccessTokenResult> GetAccessTokenAsync(string issuedScope, bool issuedScopeDifferent, Client client, string? redirectUri = null, EndUser? endUser = null)
    {
        TokenType tokenType = await _tokenProvider.GetTokenTypeAsync(client).ConfigureAwait(false);
        IEnumerable<Scope> scopeList = await _scopeService.GetScopeListAsync(issuedScope).ConfigureAwait(false);

        string issuer = await _serverMetadataService.GetTokenIssuerAsync().ConfigureAwait(false);
        var additionalParameters = await _tokenTypeDataSource.GetTokenAdditionalParametersAsync(tokenType).ConfigureAwait(false);

        Dictionary<string, string> additionalParametersDictionary = new(additionalParameters.Count());

        foreach (var additionalParameter in additionalParameters)
        {
            additionalParametersDictionary.Add(additionalParameter.Name, additionalParameter.Value);
        }

        IEnumerable<Resource> resources = await _resourceService.GetResourcesByScopesAsync(scopeList).ConfigureAwait(false);

        IEnumerable<string> audiences = resources.Select(x => x.Name);

        int? tokenExpirationSeconds = client.TokenExpirationSeconds ?? _options.Value.Tokens?.DefaultTokenExpirationSeconds;
        DateTime currentDateTime = _dateTimeService.GetCurrentDateTime();
        DateTime activationDateTime = currentDateTime;
        DateTime? expirationDateTime = tokenExpirationSeconds is not null ? currentDateTime.AddSeconds(Convert.ToDouble(tokenExpirationSeconds.Value)) : null;

        TokenContext tokenContext = new()
        {
            Scopes = scopeList,
            Client = client,
            CreationDateTime = currentDateTime,
            ActivationDateTime = activationDateTime,
            ExpirationDateTime = expirationDateTime,
            LifetimeSeconds = tokenExpirationSeconds,
            Issuer = issuer,
            AdditionalParameters = additionalParametersDictionary,
            Audiences = audiences,
            EndUser = endUser,
            RedirectUri = redirectUri
        };

        string accessTokenValue = await _tokenProvider.GetTokenValueAsync(tokenType, tokenContext).ConfigureAwait(false);

        AccessTokenResult accessToken = new()
        {
            ClientId = client.ClientId,
            Username = endUser?.Username,
            ExpiresIn = tokenExpirationSeconds,
            IssueDateTime = currentDateTime,
            ExpirationDateTime = expirationDateTime,
            Scope = issuedScope,
            IssuedScopeDifferent = issuedScopeDifferent,
            RedirectUri = redirectUri,
            Value = accessTokenValue,
            Type = tokenType.Name
        };

        await _tokenStorage.AddAccessTokenAsync(accessToken).ConfigureAwait(false);

        return accessToken;
    }
}
