// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ServerInformation;
using ExtDotNet.OAuth20.Server.Default.ServerInformation;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace ExtDotNet.OAuth20.Server.ServiceCollections;

public static class IServerInformationServiceCollectionExtensions
{
    public static IServiceCollection SetOAuth20ServerInformation(this IServiceCollection services)
    {
        services.AddSingleton<IServerInformationMetadata, DefaultServerInformationMetadata>();

        services.SetDefaultInformation();

        services.AddSingleton<IServerInformationService, DefaultServerInformationService>();

        return services;
    }

    public static IServiceCollection SetDefaultInformation(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;

        var serverInformationMetadata = services.BuildServiceProvider().GetRequiredService<IServerInformationMetadata>();

        if (options.ServerInformation?.ScopeAdditional?.Any() is true)
        {
            if (serverInformationMetadata.ScopeAdditional is null)
            {
                serverInformationMetadata.ScopeAdditional = new ConcurrentDictionary<string, string>();
            }

            foreach (var scopeInformationItem in options.ServerInformation.ScopeAdditional)
            {
                serverInformationMetadata.ScopeAdditional.Add(scopeInformationItem);
            }
        }

        serverInformationMetadata.ScopeDefaultValue = options.ServerInformation?.ScopeDefaultValue;
        serverInformationMetadata.ScopeRequirements = options.ServerInformation?.ScopeRequirements;

        if (options.ServerInformation?.AuthorizationCodeAdditional?.Any() is true)
        {
            if (serverInformationMetadata.AuthorizationCodeAdditional is null)
            {
                serverInformationMetadata.AuthorizationCodeAdditional = new ConcurrentDictionary<string, string>();
            }

            foreach (var authorizationCodeInformationItem in options.ServerInformation.AuthorizationCodeAdditional)
            {
                serverInformationMetadata.AuthorizationCodeAdditional.Add(authorizationCodeInformationItem);
            }
        }

        if (options.ServerInformation?.AuthorizationCodeSizeSymbols is null)
        {
            // TODO: a more advanced determination of the authorization code's length.
            int authorizationCodeSizeSymbols = Guid.NewGuid().ToString("N").Length * 2;

            serverInformationMetadata.AuthorizationCodeSizeSymbols = authorizationCodeSizeSymbols.ToString();
        }

        services.AddSingleton(serverInformationMetadata);

        return services;
    }
}
