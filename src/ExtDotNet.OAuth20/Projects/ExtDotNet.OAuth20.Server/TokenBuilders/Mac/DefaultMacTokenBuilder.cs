// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.TokenBuilders.Mac;

/// <summary>
/// Description RFC7519: <see cref="https://datatracker.ietf.org/doc/html/draft-ietf-oauth-v2-http-mac-05"/>
/// </summary>
public class DefaultMacTokenBuilder : IMacTokenBuilder
{
    public Task<string> BuildTokenAsync(TokenContext tokenBuilderContext)
    {
        throw new NotImplementedException();
    }
}
