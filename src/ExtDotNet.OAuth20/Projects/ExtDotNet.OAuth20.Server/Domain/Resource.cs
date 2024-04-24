// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class Resource : Int32IdEntityBase, INamedEntity
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public IEnumerable<Scope>? Scopes { get; set; }

    public IEnumerable<ResourceSigningCredentialsAlgorithm>? SigningCredentialsAlgorithms { get; set; }
}
