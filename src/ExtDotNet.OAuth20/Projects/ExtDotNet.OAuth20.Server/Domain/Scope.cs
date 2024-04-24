// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class Scope : Int32IdEntityBase, INamedEntity
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public int ResourceId { get; set; }

    public Resource Resource { get; set; } = default!;

    public IEnumerable<ClientScope>? ClientScopes { get; set; }
}
