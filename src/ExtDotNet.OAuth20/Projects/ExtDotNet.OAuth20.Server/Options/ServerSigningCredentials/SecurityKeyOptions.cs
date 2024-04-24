// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Options.ServerSigningCredentials.Enumerations;

namespace ExtDotNet.OAuth20.Server.Options.ServerSigningCredentials;

public class SecurityKeyOptions
{
    public int? SecurityKeySize { get; set; }

    public string? SecurityKeyId { get; set; }

    public SecurityAlgorithmType SecurityAlgorithmType { get; set; }
}
