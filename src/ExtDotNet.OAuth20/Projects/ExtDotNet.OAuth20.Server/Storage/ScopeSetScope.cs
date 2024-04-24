// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Storage;

public class ScopeSetScope : EntityBase<int>
{
    public int ScopeSetId { get; set; }

    public ScopeSet ScopeSet { get; set; } = default!;

    public int ScopeId { get; set; }

    public Scope Scope { get; set; } = default!;
}
