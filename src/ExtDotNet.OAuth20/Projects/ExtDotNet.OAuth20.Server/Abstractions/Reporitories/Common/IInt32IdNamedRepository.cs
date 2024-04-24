// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Abstractions.Reporitories.Common;

public interface IInt32IdNamedRepository<TEntity> : IInt32IdRepository<TEntity>, INamedRepository<TEntity, int>
    where TEntity : Int32IdEntityBase, INamedEntity, new()
{
}
