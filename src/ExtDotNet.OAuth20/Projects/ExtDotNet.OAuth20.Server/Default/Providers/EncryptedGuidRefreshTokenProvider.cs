// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Providers;
using ExtDotNet.OAuth20.Server.Models;
using ExtDotNet.OAuth20.Server.Options;
using ExtDotNet.OAuth20.Server.Utilities;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Providers;

public class EncryptedGuidRefreshTokenProvider(IOptions<OAuth20ServerOptions> options) : IRefreshTokenProvider
{
    private readonly string? _encryptionKey = options.Value?.Flows?.RefreshTokenEncryptionKey;

    public ValueTask<string> GetRefreshTokenValueAsync(AccessTokenResult _) =>
        ValueTask.FromResult(EncryptionUtilities.EncryptString(Guid.NewGuid().ToString("N"), _encryptionKey));
}
