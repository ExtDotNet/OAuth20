// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Default.Flows;
using ExtDotNet.OAuth20.Server.Flows;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ExtDotNet.OAuth20.Server.ServiceCollections;

public static class IFlowServiceCollectionExtensions
{
    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4
    /// </summary>
    public static IServiceCollection SetOAuth20Flows(this IServiceCollection services)
    {
        services.AddSingleton<IFlowMetadataCollection, DefaultFlowMetadataCollection>();

        services.SetOAuth20DefaultFlows();
        services.SetOAuth20FlowsFromConfiguration();

        services.AddScoped<IFlowProvider, DefaultFlowProvider>();
        services.AddScoped<IFlowRouter, DefaultFlowRouter>();

        return services;
    }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-8.3
    /// </summary>
    public static IServiceCollection SetOAuth20FlowsFromConfiguration(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;

        if (options.Flows?.FlowList?.Any() is not true) return services;

        foreach (var flowOptions in options.Flows.FlowList)
        {
            if (flowOptions.Implementation is null)
            {
                continue;
            }

            if (flowOptions.Abstraction is null || !TryGetType(flowOptions.Abstraction.AssemblyName, flowOptions.Abstraction.TypeName, out Type? abstractionType))
            {
                if (!TryGetType(services, flowOptions.GrantTypeName, flowOptions.ResponseTypeName, out abstractionType))
                {
                    continue;
                }
            }

            var flowMetadata = FlowMetadata.Create(flowOptions.Name, flowOptions.GrantTypeName, flowOptions.ResponseTypeName, abstractionType!, flowOptions.Description);

            if (!TryGetType(flowOptions.Implementation.AssemblyName, flowOptions.Implementation.TypeName, out Type? implementationType))
            {
                continue;
            }

            services.SetOAuth20Flow(flowMetadata, implementationType!);
        }

        return services;
    }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-8.3
    /// </summary>
    public static IServiceCollection SetOAuth20Flow<TAbstraction, TImplementation>(this IServiceCollection services, string name, string grantTypeName, string responseTypeName, string? description = null)
        where TImplementation : TAbstraction
        where TAbstraction : IFlow
        => services.SetOAuth20Flow(name, grantTypeName, responseTypeName, typeof(TAbstraction), typeof(TImplementation), description);

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-8.3
    /// </summary>
    public static IServiceCollection SetOAuth20Flow(this IServiceCollection services, string name, string grantTypeName, string responseTypeName, Type abstraction, Type implementation, string? description = null)
       => services.SetOAuth20Flow(FlowMetadata.Create(name, grantTypeName, responseTypeName, abstraction, description), implementation);

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-8.3
    /// </summary>
    public static IServiceCollection SetOAuth20Flow(this IServiceCollection services, FlowMetadata flowMetadata, Type implementation)
    {
        services.SetOAuth20Flow(flowMetadata);
        services.AddScoped(flowMetadata.Abstraction, implementation);

        return services;
    }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-8.3
    /// </summary>
    public static IServiceCollection SetOAuth20Flow<TImplementation>(this IServiceCollection services, FlowMetadata flowMetadata)
        where TImplementation : IFlow
        => services.SetOAuth20Flow(flowMetadata, typeof(TImplementation));

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4
    /// </summary>
    private static IServiceCollection SetOAuth20DefaultFlows(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;

        services.SetOAuth20DefaultFlow<IAuthorizationCodeFlow, DefaultAuthorizationCodeFlow>(
            name: options.Flows?.AuthorizationCodeFlowName ?? "authorization_code",
            defaultGrantTypeName: options.Flows?.AuthorizationFlowGrantTypeName ?? "authorization_code",
            defaultResponseTypeName: options.Flows?.AuthorizationCodeFlowResponseTypeName ?? "code",
            defaultDescription: "Authorization code flow");

        services.SetOAuth20DefaultFlow<IClientCredentialsFlow, DefaultClientCredentialsFlow>(
            name: options.Flows?.ClientCredentialsFlowName ?? "client_credentials",
            defaultGrantTypeName: options.Flows?.ClientCredentialsFlowGrantTypeName ?? "client_credentials",
            defaultResponseTypeName: null,
            defaultDescription: "Client credentials flow");

        services.SetOAuth20DefaultFlow<IImplicitFlow, DefaultImplicitFlow>(
            name: options.Flows?.ImplicitFlowName ?? "implicit",
            defaultGrantTypeName: null,
            defaultResponseTypeName: options.Flows?.ImplicitFlowResponseTypeName ?? "token",
            defaultDescription: "Implicit flow");

        services.SetOAuth20DefaultFlow<IResourceOwnerPasswordCredentialsFlow, DefaultResourceOwnerPasswordCredentialsFlow>(
            name: options.Flows?.ResourceOwnerPasswordCredentialsFlowName ?? "resource_owner_password_credentials",
            defaultGrantTypeName: options.Flows?.ResourceOwnerPasswordCredentialsFlowGrantTypeName ?? "password",
            defaultResponseTypeName: null,
            defaultDescription: "Resource owner password credentials flow");

        services.SetOAuth20DefaultFlow<IRefreshTokenFlow, DefaultRefreshTokenFlow>(
            name: options.Flows?.RefreshTokenFlowName ?? "refresh_token",
            defaultGrantTypeName: options.Flows?.RefreshTokenFlowGrantTypeName ?? "refresh_token",
            defaultResponseTypeName: null,
            defaultDescription: "Refresh token flow");

        return services;
    }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-8.3
    /// </summary>
    private static IServiceCollection SetOAuth20DefaultFlow<TDefaultAbstraction, TDefaultImplementation>(this IServiceCollection services, string name, string? defaultGrantTypeName, string? defaultResponseTypeName, string? defaultDescription = null)
        where TDefaultImplementation : TDefaultAbstraction
        where TDefaultAbstraction : IFlow
        => services.SetOAuth20DefaultFlow(name, defaultGrantTypeName, defaultResponseTypeName, typeof(TDefaultAbstraction), typeof(TDefaultImplementation), defaultDescription);

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-8.3
    /// </summary>
    private static IServiceCollection SetOAuth20DefaultFlow(this IServiceCollection services, string name, string? defaultGrantTypeName, string? defaultResponseTypeName, Type defaultAbstraction, Type defaultImplementation, string? defaultDescription = null)
        => services.SetOAuth20Flow(FlowMetadata.Create(name, defaultGrantTypeName, defaultResponseTypeName, defaultAbstraction, defaultDescription), defaultImplementation);

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-8.3
    /// </summary>
    private static IServiceCollection SetOAuth20Flow(this IServiceCollection services, FlowMetadata flowMetadata)
    {
        var flowMetadataCollection = services.BuildServiceProvider().GetRequiredService<IFlowMetadataCollection>();

        if (flowMetadata.GrantTypeName is not null)
        {
            if (!flowMetadata.Abstraction.IsAssignableTo(typeof(ITokenFlow)))
            {
                throw new InvalidOperationException($"{nameof(flowMetadata.Abstraction)}:{flowMetadata.Abstraction.Name} should be inherited from {nameof(ITokenFlow)}");
            }

            flowMetadataCollection.FlowsWithGrantType[flowMetadata.GrantTypeName] = flowMetadata;
        }

        if (flowMetadata.ResponseTypeName is not null)
        {
            if (!flowMetadata.Abstraction.IsAssignableTo(typeof(IAuthorizeFlow)))
            {
                throw new InvalidOperationException($"{nameof(flowMetadata.Abstraction)}:{flowMetadata.Abstraction.Name} should be inherited from {nameof(IAuthorizeFlow)}");
            }

            flowMetadataCollection.FlowsWithResponseType[flowMetadata.ResponseTypeName] = flowMetadata;
        }

        flowMetadataCollection.Flows[flowMetadata.Name] = flowMetadata;

        services.AddSingleton(flowMetadataCollection);

        return services;
    }

    private static bool TryGetType(IServiceCollection services, string? grantTypeName, string? responseTypeName, out Type? type)
    {
        var flowMetadataCollection = services.BuildServiceProvider().GetRequiredService<IFlowMetadataCollection>();

        if (grantTypeName is not null && flowMetadataCollection.FlowsWithGrantType.TryGetValue(grantTypeName, out FlowMetadata? flowMetadata) ||
            responseTypeName is not null && flowMetadataCollection.FlowsWithResponseType.TryGetValue(responseTypeName, out flowMetadata))
        {
            type = flowMetadata.Abstraction;
            return true;
        }
        else
        {
            type = null;
            return false;
        }
    }

    private static bool TryGetType(string assemblyName, string typeName, out Type? type)
    {
        Assembly? asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == assemblyName);
        type = asm?.GetTypes().FirstOrDefault(x => x.Name == typeName);

        if (type is not null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
