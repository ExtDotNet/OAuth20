// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.Abstractions.Providers;

public interface IRefreshTokenProvider
{
    public ValueTask<string> GetRefreshTokenValueAsync(AccessTokenResult accessToken);
}
