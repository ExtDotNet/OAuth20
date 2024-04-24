// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Abstractions.Reporitories.Common;

public interface IRepository<TEntity, TIdentifier>
    where TEntity : EntityBase<TIdentifier>, new()
{
    public Task<TIdentifier> AddAsync(TEntity entity);

    public Task UpdateAsync(TEntity entity);

    public Task DeleteAsync(TIdentifier id);

    public Task<TEntity?> GetByIdAsync(TIdentifier id);

    public Task<IEnumerable<TEntity>> GetAllAsync(int limit = 0, int offset = 0);
}
