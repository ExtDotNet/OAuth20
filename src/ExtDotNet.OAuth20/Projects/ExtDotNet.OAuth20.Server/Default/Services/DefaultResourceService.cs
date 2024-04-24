// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultResourceService : IResourceService
{
    private readonly IResourceDataSource _resourceDataSource;

    public DefaultResourceService(IResourceDataSource resourceDataSource)
    {
        _resourceDataSource = resourceDataSource;
    }

    public async Task<Resource> GetResourceByScopeAsync(Scope scope)
    {
        return await _resourceDataSource.GetResourceByScopeAsync(scope);
    }

    public async Task<IEnumerable<Resource>> GetResourcesByScopesAsync(IEnumerable<Scope> scopes)
    {
        Dictionary<int, Resource> resources = new();

        foreach (var scope in scopes)
        {
            if (!resources.TryGetValue(scope.ResourceId, out var _))
            {
                resources[scope.ResourceId] = await _resourceDataSource.GetResourceByScopeAsync(scope);
            }
        }

        return resources.Select(x => x.Value);
    }

    public async Task<IEnumerable<SigningCredentialsAlgorithm>> GetResourceSigningCredentialsAlgorithmsAsync(Resource resource)
    {
        return await _resourceDataSource.GetResourceSigningCredentialsAlgorithmsAsync(resource);
    }
}
