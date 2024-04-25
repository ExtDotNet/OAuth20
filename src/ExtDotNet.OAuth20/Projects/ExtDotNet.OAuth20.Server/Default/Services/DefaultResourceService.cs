// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultResourceService(IResourceDataSource resourceDataSource) : IResourceService
{
    private readonly IResourceDataSource _resourceDataSource = resourceDataSource ?? throw new ArgumentNullException(nameof(resourceDataSource));

    public async Task<Resource> GetResourceByScopeAsync(Scope scope) =>
        await _resourceDataSource.GetResourceByScopeAsync(scope).ConfigureAwait(false);

    public async Task<IEnumerable<Resource>> GetResourcesByScopesAsync(IEnumerable<Scope> scopes)
    {
        Dictionary<int, Resource> resources = [];

        foreach (var scope in scopes)
        {
            if (!resources.TryGetValue(scope.ResourceId, out var _))
            {
                resources[scope.ResourceId] = await _resourceDataSource.GetResourceByScopeAsync(scope).ConfigureAwait(false);
            }
        }

        return resources.Select(x => x.Value);
    }

    public async Task<IEnumerable<SigningCredentialsAlgorithm>> GetResourceSigningCredentialsAlgorithmsAsync(Resource resource) =>
        await _resourceDataSource.GetResourceSigningCredentialsAlgorithmsAsync(resource).ConfigureAwait(false);
}
