// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IResourceService
{
    public Task<Resource> GetResourceByScopeAsync(Scope scope);

    public Task<IEnumerable<Resource>> GetResourcesByScopesAsync(IEnumerable<Scope> scopes);

    public Task<IEnumerable<SigningCredentialsAlgorithm>> GetResourceSigningCredentialsAlgorithmsAsync(Resource resource);
}
