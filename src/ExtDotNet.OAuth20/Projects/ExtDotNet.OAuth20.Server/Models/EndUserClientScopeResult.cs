// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Models;

public class EndUserClientScopeResult
{
    public string Username { get; set; } = default!;

    public string ClientId { get; set; } = default!;

    public string ServerAllowedScope { get; set; } = default!;

    public string EndUserIssuedScope { get; set; } = default!;

    public bool EndUserIssuedScopeDifferent { get; set; } = false;
}
