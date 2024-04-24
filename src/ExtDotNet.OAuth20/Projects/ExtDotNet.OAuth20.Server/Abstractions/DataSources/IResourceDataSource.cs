// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.DataSources;

public interface IResourceDataSource
{
    public Task<Resource> GetResourceByScopeAsync(Scope scope);

    public Task<IEnumerable<SigningCredentialsAlgorithm>> GetResourceSigningCredentialsAlgorithmsAsync(Resource resource);
}
