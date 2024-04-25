// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Interceptors;
using ExtDotNet.OAuth20.Server.Default.Interceptors;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.ServiceCollections;

public static class IInterceptorServiceCollectionExtensions
{
    public static IServiceCollection SetOAuth20LoggingScopeInterceptor(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;

        bool enableInterceptors = options.EnableLoggingScopeInterceptor;

        if (enableInterceptors)
        {
            services.AddScoped<IScopeInterceptor, LoggingScopeInterceptor>();
        }

        return services;
    }
}
