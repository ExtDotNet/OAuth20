// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;
using ExtDotNet.OAuth20.Server.Abstractions.ServerInformation;

namespace ExtDotNet.OAuth20.Server.Default.ServerInformation;

public class DefaultServerInformationService : IServerInformationService
{
    private readonly IServerInformationMetadata _serverInformationMetadata;

    public DefaultServerInformationService(IServerInformationMetadata serverInformationMetadata)
    {
        _serverInformationMetadata = serverInformationMetadata;
    }

    public Task<IDictionary<string, string>?> GetAuthorizationCodeAdditionalInformationAsync()
    {
        return Task.FromResult(_serverInformationMetadata.AuthorizationCodeAdditional);
    }

    public Task<string> GetAuthorizationCodeSizeSymbolsInformationAsync()
    {
        if (_serverInformationMetadata.AuthorizationCodeSizeSymbols is null)
        {
            throw new ServerConfigurationErrorException(
                "The client should avoid making assumptions about code " +
                "value sizes. The authorization server SHOULD document " +
                "the size of any value it issues.");
        }

        return Task.FromResult(_serverInformationMetadata.AuthorizationCodeSizeSymbols);
    }

    public Task<IDictionary<string, string>?> GetScopeAdditionalInformationAsync()
    {
        return Task.FromResult(_serverInformationMetadata.ScopeAdditional);
    }

    public Task<string?> GetScopeDefaultValueInformationAsync()
    {
        return Task.FromResult(_serverInformationMetadata.ScopeDefaultValue);
    }

    public Task<string?> GetScopeRequirementsInformationAsync()
    {
        return Task.FromResult(_serverInformationMetadata.ScopeRequirements);
    }
}
