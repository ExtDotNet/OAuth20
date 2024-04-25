// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;
using ExtDotNet.OAuth20.Server.Abstractions.ServerInformation;

namespace ExtDotNet.OAuth20.Server.Default.ServerInformation;

public class DefaultServerInformationService(IServerInformationMetadata serverInformationMetadata) : IServerInformationService
{
    private readonly IServerInformationMetadata _serverInformationMetadata = serverInformationMetadata ??
            throw new ArgumentNullException(nameof(serverInformationMetadata));

    public ValueTask<IDictionary<string, string>?> GetAuthorizationCodeAdditionalInformationAsync() =>
        ValueTask.FromResult(_serverInformationMetadata.AuthorizationCodeAdditional);

    public ValueTask<string> GetAuthorizationCodeSizeSymbolsInformationAsync()
    {
        if (_serverInformationMetadata.AuthorizationCodeSizeSymbols is null)
        {
            throw new ServerConfigurationErrorException(
                "The client should avoid making assumptions about code " +
                "value sizes. The authorization server SHOULD document " +
                "the size of any value it issues.");
        }

        return ValueTask.FromResult(_serverInformationMetadata.AuthorizationCodeSizeSymbols);
    }

    public ValueTask<IDictionary<string, string>?> GetScopeAdditionalInformationAsync() =>
        ValueTask.FromResult(_serverInformationMetadata.ScopeAdditional);

    public ValueTask<string?> GetScopeDefaultValueInformationAsync() =>
        ValueTask.FromResult(_serverInformationMetadata.ScopeDefaultValue);

    public ValueTask<string?> GetScopeRequirementsInformationAsync() =>
        ValueTask.FromResult(_serverInformationMetadata.ScopeRequirements);
}
