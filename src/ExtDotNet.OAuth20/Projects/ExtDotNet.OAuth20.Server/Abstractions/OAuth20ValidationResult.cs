// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions;

public class OAuth20ValidationResult
{
    public bool Success { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }
}
