// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.WebPageBuilders;
using ExtDotNet.OAuth20.Server.Default.WebPageBuilders;

namespace ExtDotNet.OAuth20.Server.ServiceCollections;

public static class IWebPageBuilderServiceCollectionExtensions
{
    public static IServiceCollection SetOAuth20WebPageBuilderServices(this IServiceCollection services)
    {
        services.AddSingleton<IWebPageBuilderMetadataCollection, DefaultWebPageBuilderMetadataCollection>();

        services.AddScoped<IWebPageBuilderProvider, DefaultWebPageBuilderProvider>();
        services.AddScoped<IWebPageBuilderRouter, DefaultWebPageBuilderRouter>();

        return services;
    }

    public static IServiceCollection SetOAuth20WebPageBuilder<TAbstraction, TImplementation>(this IServiceCollection services, string route, string? description = null)
        where TImplementation : TAbstraction
        where TAbstraction : IWebPageBuilder
        => services.SetOAuth20WebPageBuilder(route, typeof(TAbstraction), typeof(TImplementation), description);

    public static IServiceCollection SetOAuth20WebPageBuilder(this IServiceCollection services, string route, Type abstraction, Type implementation, string? description = null)
        => services.SetOAuth20WebPageBuilder(WebPageBuilderMetadata.Create(route, abstraction, description), implementation);

    public static IServiceCollection SetOAuth20WebPageBuilder(this IServiceCollection services, WebPageBuilderMetadata webPageBuilderMetadata, Type implementation)
    {
        services.SetOAuth20WebPageBuilderMetadata(webPageBuilderMetadata);
        services.AddScoped(webPageBuilderMetadata.Abstraction, implementation);

        return services;
    }

    public static IServiceCollection SetOAuth20WebPageBuilder<TImplementation>(this IServiceCollection services, WebPageBuilderMetadata webPageBuilderMetadata)
        where TImplementation : IWebPageBuilder
        => services.SetOAuth20WebPageBuilder(webPageBuilderMetadata, typeof(TImplementation));

    private static IServiceCollection SetOAuth20WebPageBuilderMetadata(this IServiceCollection services, WebPageBuilderMetadata webPageBuilderMetadata)
    {
        var webPageBuilderMetadataCollection = services.BuildServiceProvider().GetRequiredService<IWebPageBuilderMetadataCollection>();

        webPageBuilderMetadataCollection.WebPageBuilders[webPageBuilderMetadata.Route] = webPageBuilderMetadata;

        services.AddSingleton(webPageBuilderMetadataCollection);

        return services;
    }
}
