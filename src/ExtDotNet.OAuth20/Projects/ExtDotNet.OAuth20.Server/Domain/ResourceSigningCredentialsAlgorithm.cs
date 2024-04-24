// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class ResourceSigningCredentialsAlgorithm : Int32IdEntityBase
{
    public int ResourceId { get; set; }

    public Resource Resource { get; set; } = default!;

    public int SigningCredentialsAlgorithmId { get; set; }

    public SigningCredentialsAlgorithm SigningCredentialsAlgorithm { get; set; } = default!;
}
