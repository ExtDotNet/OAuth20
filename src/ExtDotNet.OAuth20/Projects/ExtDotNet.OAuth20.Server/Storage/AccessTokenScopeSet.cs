// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Storage;

public class AccessTokenScopeSet : EntityBase<int>
{
    public int AccessTokenId { get; set; }

    public AccessToken AccessToken { get; set; } = default!;

    public int ScopeSetId { get; set; }

    public ScopeSet ScopeSet { get; set; } = default!;
}
