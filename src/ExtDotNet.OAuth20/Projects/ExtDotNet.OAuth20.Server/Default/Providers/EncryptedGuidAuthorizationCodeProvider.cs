// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Providers;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models.Flows.AuthorizationCode.Authorize;
using ExtDotNet.OAuth20.Server.Options;
using ExtDotNet.OAuth20.Server.Utilities;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Providers;

public class EncryptedGuidAuthorizationCodeProvider(IOptions<OAuth20ServerOptions> options) : IAuthorizationCodeProvider
{
    private readonly string? _encryptionKey = options.Value?.Flows?.AuthorizationCodeEncryptionKey;

    public ValueTask<string> GetAuthorizationCodeValueAsync(AuthorizeArguments args, EndUser endUser, Client client, string redirectUri, string scope) =>
        ValueTask.FromResult(EncryptionUtilities.EncryptString(Guid.NewGuid().ToString("N"), _encryptionKey));
}
