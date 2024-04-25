// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataStorages;
using ExtDotNet.OAuth20.Server.Abstractions.Providers;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultRefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenStorage _refreshTokenStorage;
    private readonly IRefreshTokenProvider _refreshTokenProvider;

    public DefaultRefreshTokenService(IRefreshTokenStorage refreshTokenStorage, IRefreshTokenProvider refreshTokenProvider)
    {
        _refreshTokenStorage = refreshTokenStorage;
        _refreshTokenProvider = refreshTokenProvider;
    }

    public async Task<RefreshTokenResult> GetRefreshTokenAsync(AccessTokenResult accessToken)
    {
        string refreshTokenValue = await _refreshTokenProvider.GetRefreshTokenValueAsync(accessToken).ConfigureAwait(false);

        RefreshTokenResult refreshToken = new()
        {
            Value = refreshTokenValue,
            AccessTokenValue = accessToken.Value
        };

        await _refreshTokenStorage.AddRefreshTokenAsync(refreshToken).ConfigureAwait(false);

        return refreshToken;
    }
}
