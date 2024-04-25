// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;

namespace ExtDotNet.OAuth20.Server.ServiceCollections;

public static class IDataSourceServiceCollectionExtensions
{
    public static IServiceCollection SetOAuth20DataSources(this IServiceCollection services, IDataSourceContext dataSourceContext)
    {
        services.AddScoped(typeof(IClientDataSource), dataSourceContext.ClientDataSourceType);
        services.AddScoped(typeof(IClientSecretDataSource), dataSourceContext.ClientSecretDataSourceType);
        services.AddScoped(typeof(IEndUserDataSource), dataSourceContext.EndUserDataSourceType);
        services.AddScoped(typeof(IFlowDataSource), dataSourceContext.FlowDataSourceType);
        services.AddScoped(typeof(IResourceDataSource), dataSourceContext.ResourceDataSourceType);
        services.AddScoped(typeof(IScopeDataSource), dataSourceContext.ScopeDataSourceType);
        services.AddScoped(typeof(ITokenTypeDataSource), dataSourceContext.TokenTypeDataSourceType);

        return services;
    }
}
