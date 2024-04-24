// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Storage;

public class ScopeSet : EntityBase<int>
{
    public string Value { get; set; } = default!;

    public IEnumerable<AccessTokenScopeSet>? AccessTokenScopeSets { get; set; }

    public IEnumerable<ScopeSetScope>? ScopeSetScopes { get; set; }
}
