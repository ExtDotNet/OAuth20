// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultSigningCredentialsAlgorithmsService : ISigningCredentialsAlgorithmsService
{
    private readonly IResourceService _resourceService;

    public DefaultSigningCredentialsAlgorithmsService(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    public async Task<IEnumerable<SigningCredentialsAlgorithm>> GetSigningCredentialsAlgorithmsForScopesAsync(IEnumerable<Scope> scopes)
    {
        // TODO: Can be refactored
        IEnumerable<Resource> resources = await _resourceService.GetResourcesByScopesAsync(scopes).ConfigureAwait(false);

        IEnumerable<SigningCredentialsAlgorithm> signingCredentialsAlgorithms = [];

        foreach (var resource in resources)
        {
            IEnumerable<SigningCredentialsAlgorithm> resourceSigningCredentialsAlgorithms = await _resourceService
                .GetResourceSigningCredentialsAlgorithmsAsync(resource)
                .ConfigureAwait(false);

            if (!resourceSigningCredentialsAlgorithms.Any())
            {
                continue;
            }

            // TODO: figure how to get initial signing credentials out
            signingCredentialsAlgorithms = signingCredentialsAlgorithms.IntersectBy(resourceSigningCredentialsAlgorithms.Select(x => x.Id), x => x.Id);

            if (!signingCredentialsAlgorithms.Any())
            {
                throw new InvalidRequestException(
                    $"There are no registered required Signing Credentials for " +
                    $"the requested resource [{resource.Name}] in this server instance.");
            }
        }

        return signingCredentialsAlgorithms;
    }
}
