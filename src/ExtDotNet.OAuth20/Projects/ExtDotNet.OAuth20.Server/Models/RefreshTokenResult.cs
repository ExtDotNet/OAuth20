// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Models;

public class RefreshTokenResult
{
    public string Value { get; set; } = default!;

    public string AccessTokenValue { get; set; } = default!;
}
