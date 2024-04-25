// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataStorages;

namespace ExtDotNet.OAuth20.Server.ServiceCollections;

public static class IDataStorageServiceCollectionExtensions
{
    public static IServiceCollection SetOAuth20DataStorages(this IServiceCollection services, IDataStorageContext dataStorageContext)
    {
        services.AddScoped(typeof(IAccessTokenStorage), dataStorageContext.AccessTokenStorageType);
        services.AddScoped(typeof(IAuthorizationCodeStorage), dataStorageContext.AuthorizationCodeStorageType);
        services.AddScoped(typeof(IRefreshTokenStorage), dataStorageContext.RefreshTokenStorageType);
        services.AddScoped(typeof(IEndUserClientScopeStorage), dataStorageContext.EndUserClientScopeStorageType);

        return services;
    }
}
