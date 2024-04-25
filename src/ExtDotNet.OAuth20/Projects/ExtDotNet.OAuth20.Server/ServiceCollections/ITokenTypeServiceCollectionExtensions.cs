// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;
using ExtDotNet.OAuth20.Server.Default.TokenBuilders;
using ExtDotNet.OAuth20.Server.Options;
using ExtDotNet.OAuth20.Server.TokenBuilders.Basic;
using ExtDotNet.OAuth20.Server.TokenBuilders.Jwt;
using ExtDotNet.OAuth20.Server.TokenBuilders.Mac;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ExtDotNet.OAuth20.Server.ServiceCollections;

public static class ITokenTypeServiceCollectionExtensions
{
    public static IServiceCollection SetOAuth20TokenTypes(this IServiceCollection services)
    {
        services.AddSingleton<ITokenBuilderMetadataCollection, DefaultTokenBuilderMetadataCollection>();

        services.SetOAuth20DefaultTokens();
        services.SetOAuth20TokenTypesFromConfiguration();

        services.AddScoped<ITokenBuilderProvider, DefaultTokenBuilderProvider>();
        services.AddScoped<ITokenBuilderSelector, DefaultTokenBuilderSelector>();

        return services;
    }

    public static IServiceCollection SetOAuth20TokenTypesFromConfiguration(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;

        if (options.Tokens?.TokenTypes?.Any() is not true) return services;

        foreach (var tokenTypeOptions in options.Tokens.TokenTypes)
        {
            if (tokenTypeOptions.Builder is null) continue;

            if (tokenTypeOptions.Builder.Implementation is null) continue;

            if (tokenTypeOptions.Builder.Abstraction is null || !TryGetType(tokenTypeOptions.Builder.Abstraction.AssemblyName, tokenTypeOptions.Builder.Abstraction.TypeName, out Type? abstractionType))
            {
                if (!TryGetType(services, tokenTypeOptions.Name, out abstractionType)) continue;
            }

            var tokenTypeMetadata = TokenBuilderMetadata.Create(tokenTypeOptions.Name, abstractionType!, tokenTypeOptions.Description, tokenTypeOptions.AdditionalParameters);

            if (!TryGetType(tokenTypeOptions.Builder.Implementation.AssemblyName, tokenTypeOptions.Builder.Implementation.TypeName, out Type? implementationType)) continue;

            services.SetOAuth20TokenType(tokenTypeMetadata, implementationType!);
        }

        return services;
    }

    public static IServiceCollection SetOAuth20TokenType<TAbstraction, TImplementation>(this IServiceCollection services, string tokenType, string? description = null, IDictionary<string, string>? additionalParameters = null)
        where TImplementation : TAbstraction
        where TAbstraction : ITokenBuilder
        => services.SetOAuth20TokenType(tokenType, typeof(TAbstraction), typeof(TImplementation), description, additionalParameters);

    public static IServiceCollection SetOAuth20TokenType(this IServiceCollection services, string tokenType, Type abstraction, Type implementation, string? description = null, IDictionary<string, string>? additionalParameters = null)
        => services.SetOAuth20TokenType(TokenBuilderMetadata.Create(tokenType, abstraction, description, additionalParameters), implementation);

    public static IServiceCollection SetOAuth20TokenType(this IServiceCollection services, TokenBuilderMetadata tokenTypeMetadata, Type implementation)
    {
        services.SetOAuth20TokenType(tokenTypeMetadata);
        services.AddScoped(tokenTypeMetadata.Abstraction, implementation);

        return services;
    }

    public static IServiceCollection SetOAuth20TokenType<TImplementation>(this IServiceCollection services, TokenBuilderMetadata tokenTypeMetadata)
        where TImplementation : ITokenBuilder
        => services.SetOAuth20TokenType(tokenTypeMetadata, typeof(TImplementation));

    private static IServiceCollection SetOAuth20DefaultTokens(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;

        services.SetOAuth20DefaultToken<IBasicTokenBuilder, DefaultBasicTokenBuilder>(
            options.Tokens?.BasicTokenTypeName ?? "Basic",
            "Basic Token Builder",
            null);

        services.SetOAuth20DefaultToken<IJwtTokenBuilder, DefaultJwtTokenBuilder>(
            options.Tokens?.JwtTokenTypeName ?? "JWT",
            "JWT Token Builder",
            null);

        services.SetOAuth20DefaultToken<IMacTokenBuilder, DefaultMacTokenBuilder>(
            options.Tokens?.MacTokenTypeName ?? "MAC",
            "MAC Token Builder",
            null);

        return services;
    }

    private static IServiceCollection SetOAuth20DefaultToken<TDefaultAbstraction, TDefaultImplementation>(this IServiceCollection services, string tokenType, string? defaultDescription = null, IDictionary<string, string>? additionalParameters = null)
        where TDefaultImplementation : TDefaultAbstraction
        where TDefaultAbstraction : ITokenBuilder
        => services.SetOAuth20DefaultToken(tokenType, typeof(TDefaultAbstraction), typeof(TDefaultImplementation), defaultDescription, additionalParameters);

    private static IServiceCollection SetOAuth20DefaultToken(this IServiceCollection services, string tokenType, Type defaultAbstraction, Type defaultImplementation, string? defaultDescription = null, IDictionary<string, string>? additionalParameters = null)
    {
        services.SetOAuth20TokenType(TokenBuilderMetadata.Create(tokenType, defaultAbstraction, defaultDescription, additionalParameters), defaultImplementation);

        return services;
    }

    private static IServiceCollection SetOAuth20TokenType(this IServiceCollection services, TokenBuilderMetadata tokenTypeMetadata)
    {
        var tokenTypeMetadataCollection = services.BuildServiceProvider().GetRequiredService<ITokenBuilderMetadataCollection>();

        tokenTypeMetadataCollection.TokenBuilders[tokenTypeMetadata.TokenType] = tokenTypeMetadata;

        services.AddSingleton(tokenTypeMetadataCollection);

        return services;
    }

    private static bool TryGetType(IServiceCollection services, string tokenType, out Type? type)
    {
        var tokenTypeMetadataCollection = services.BuildServiceProvider().GetRequiredService<ITokenBuilderMetadataCollection>();

        if (tokenTypeMetadataCollection.TokenBuilders.TryGetValue(tokenType, out TokenBuilderMetadata? tokenTypeMetadata))
        {
            type = tokenTypeMetadata.Abstraction;
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

        return type is not null;
    }
}
