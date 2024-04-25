// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataStorages;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Token;
using ExtDotNet.OAuth20.Server.Abstractions.Providers;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Authorize;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultAuthorizationCodeService(
    IAuthorizationCodeStorage authorizationCodeStorage,
    IAccessTokenService accessTokenService,
    IDateTimeService dateTimeService,
    IEndUserService endUserService,
    IAuthorizationCodeProvider authorizationCodeProvider,
    IOptions<OAuth20ServerOptions> options) : IAuthorizationCodeService
{
    private readonly IAuthorizationCodeStorage _authorizationCodeStorage = authorizationCodeStorage ?? throw new ArgumentNullException(nameof(authorizationCodeStorage));
    private readonly IAccessTokenService _accessTokenService = accessTokenService ?? throw new ArgumentNullException(nameof(accessTokenService));
    private readonly IDateTimeService _dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
    private readonly IEndUserService _endUserService = endUserService ?? throw new ArgumentNullException(nameof(endUserService));
    private readonly IAuthorizationCodeProvider _authorizationCodeProvider = authorizationCodeProvider ?? throw new ArgumentNullException(nameof(authorizationCodeProvider));
    private readonly IOptions<OAuth20ServerOptions> _options = options ?? throw new ArgumentNullException(nameof(options));

    public async Task<string> GetAuthorizationCodeAsync(AuthorizeArguments args, EndUser endUser, Client client, string redirectUri, string issuedScope, bool issuedScopeDifferent)
    {
        DateTime currentDateTime = _dateTimeService.GetCurrentDateTime();
        string authorizationCodeValue = await _authorizationCodeProvider
            .GetAuthorizationCodeValueAsync(args, endUser, client, redirectUri, issuedScope)
            .ConfigureAwait(false);

        AuthorizationCodeResult authorizationCode = new()
        {
            ClientId = client.ClientId,
            Username = endUser.Username,
            Exchanged = false,
            ExpiresIn = _options.Value.DefaultAuthorizationCodeExpirationSeconds,
            IssueDateTime = currentDateTime,
            ExpirationDateTime = _options.Value.DefaultAuthorizationCodeExpirationSeconds is not null ? currentDateTime.AddSeconds(_options.Value.DefaultAuthorizationCodeExpirationSeconds.Value) : null,
            Scope = issuedScope,
            IssuedScopeDifferent = issuedScopeDifferent,
            RedirectUri = redirectUri,
            Value = authorizationCodeValue
        };

        await _authorizationCodeStorage.AddAuthorizationCodeResultAsync(authorizationCode).ConfigureAwait(false);

        return authorizationCode.Value;
    }

    public async Task<AccessTokenResult> ExchangeAuthorizationCodeAsync(string code, Client client, string? redirectUri)
    {
        AuthorizationCodeResult? authorizationCode = await _authorizationCodeStorage.GetAuthorizationCodeResultAsync(code).ConfigureAwait(false);
        if (authorizationCode is null)
        {
            throw new InvalidGrantException($"Authorization Code [{code}] is invalid and does not exist in the system.");
        }

        if (authorizationCode.ExpirationDateTime is not null)
        {
            DateTime currentDateTime = _dateTimeService.GetCurrentDateTime();
            if (authorizationCode.ExpirationDateTime >= currentDateTime)
            {
                throw new InvalidGrantException(
                    $"Authorization Code [{code}] is expired currently, " +
                    $"at [{_dateTimeService.ConvertDateTimeToString(currentDateTime)}] because its expiration time " +
                    $"is [{_dateTimeService.ConvertDateTimeToString(authorizationCode.ExpirationDateTime.Value)}].");
            }
        }

        if (authorizationCode.ClientId != client.ClientId)
        {
            throw new InvalidGrantException($"Authorization Code [{code}] was issued to another Client, not [{client.ClientId}].");
        }

        if (authorizationCode.RedirectUri != redirectUri)
        {
            throw new InvalidGrantException($"Passed Redirect URI [{redirectUri}] of the Authorization Code [{code}] does not match the Redirect URI of the Authorization Code [{code}].");
        }

        EndUser? endUser = await _endUserService.GetEndUserAsync(authorizationCode.Username).ConfigureAwait(false);
        if (endUser is null)
        {
            throw new InvalidOperationException($"Authorization Code [{code}] is bind to the EndUser with the username [{authorizationCode.Username}] that does not exist in the system.");
        }

        AccessTokenResult accessToken = await _accessTokenService
            .GetAccessTokenAsync(
                authorizationCode.Scope,
                authorizationCode.IssuedScopeDifferent,
                client,
                authorizationCode.RedirectUri,
                endUser)
            .ConfigureAwait(false);

        return accessToken;
    }
}
