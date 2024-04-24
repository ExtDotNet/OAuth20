// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Abstractions.Reporitories.Common;

public interface INamedRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
    where TEntity : EntityBase<TIdentifier>, INamedEntity, new()
{
    public Task<TEntity?> GetByNameAsync(string name);
}
