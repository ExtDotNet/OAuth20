// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class Flow : Int32IdEntityBase, INamedEntity
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public virtual string? GrantTypeName { get; set; }

    public virtual string? ResponseTypeName { get; set; }

    public IEnumerable<ClientFlow>? ClientFlows { get; set; }
}
