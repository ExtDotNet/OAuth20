// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Reporitories.Common;

public interface IRepositoryContext
{
    public void SetRepositories(IServiceCollection services);
}
