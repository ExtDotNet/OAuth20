// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Domain.Abstractions;

public abstract class EntityBase<TKey>
{
    protected EntityBase()
    {
    }

    public TKey Id { get; set; } = default!;
}
