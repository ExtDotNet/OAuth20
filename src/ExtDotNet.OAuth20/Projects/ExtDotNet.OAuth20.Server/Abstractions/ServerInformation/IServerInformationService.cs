// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.ServerInformation;

public interface IServerInformationService
{
    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.3"/>
    /// All information should be provided by a <see cref="IServerInformationService"/> instance.
    /// </summary>
    public ValueTask<IDictionary<string, string>?> GetScopeAdditionalInformationAsync();

    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.3"/>
    /// The authorization server SHOULD document its scope requirements.
    /// All information should be provided by a <see cref="IServerInformationService"/> instance.
    /// </summary>
    /// <returns></returns>
    public ValueTask<string?> GetScopeRequirementsInformationAsync();

    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.3"/>
    /// The authorization server SHOULD document its default value (if defined).
    /// All information should be provided by a <see cref="IServerInformationService"/> instance.
    /// </summary>
    /// <returns></returns>
    public ValueTask<string?> GetScopeDefaultValueInformationAsync();

    /// <summary>
    /// All information should be provided by a
    /// <see cref="IServerInformationService"/> instance.
    /// </summary>
    public ValueTask<IDictionary<string, string>?> GetAuthorizationCodeAdditionalInformationAsync();

    /// <summary>
    /// The client should avoid making assumptions about code
    /// value sizes. The authorization server SHOULD document the size of
    /// any value it issues. All information should be provided by a
    /// <see cref="IServerInformationService"/> instance.
    /// </summary>
    public ValueTask<string> GetAuthorizationCodeSizeSymbolsInformationAsync();
}
