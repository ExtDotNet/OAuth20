// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Reporitories.Common;
using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.Reporitories;

public interface IClientTypeRepository : INamedRepository<ClientType, Domain.Enums.ClientType>
{
}
