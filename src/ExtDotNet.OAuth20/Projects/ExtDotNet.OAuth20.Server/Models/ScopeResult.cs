// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Models;

public class ScopeResult
{
    public string? RequestedScope { get; set; }

    public bool IssuedScopeDifferent { get; set; } = false;

    public string IssuedScope { get; set; } = default!;
}
