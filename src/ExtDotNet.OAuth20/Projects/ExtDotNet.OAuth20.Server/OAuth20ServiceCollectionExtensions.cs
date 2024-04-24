// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Options;

namespace ExtDotNet.OAuth20.Server;

public static class OAuth20ServiceCollectionExtensions
{
    public static IServiceCollection AddOAuth20Server(this IServiceCollection services, Action<OAuth20ServerOptions>? optionsConfiguration = null)
    {
        return services;
    }
}
