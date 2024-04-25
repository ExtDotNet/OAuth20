// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ClientSecretReaders;
using ExtDotNet.OAuth20.Server.ClientSecretReaders.AuthorizationHeaderBasic;
using ExtDotNet.OAuth20.Server.ClientSecretReaders.RequestBodyClientCredentials;
using ExtDotNet.OAuth20.Server.ClientSecretReaders.TlsCertificate;
using ExtDotNet.OAuth20.Server.Default.ClientSecretReaders;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ExtDotNet.OAuth20.Server.ServiceCollections;

public static class IClientSecretServiceCollectionExtensions
{
    public static IServiceCollection SetOAuth20ClientSecretTypes(this IServiceCollection services)
    {
        services.AddSingleton<IClientSecretReaderMetadataCollection, DefaultClientSecretReaderMetadataCollection>();

        services.SetOAuth20DefaultClientSecretTypes();
        services.SetOAuth20ClientSecretTypesFromConfiguration();

        services.AddScoped<IClientSecretReaderProvider, DefaultClientSecretReaderProvider>();

        return services;
    }

    public static IServiceCollection SetOAuth20ClientSecretTypesFromConfiguration(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;

        if (options.ClientSecrets?.ClientSecretTypes?.Any() is not true) return services;

        foreach (var clientSecretTypeOptions in options.ClientSecrets.ClientSecretTypes)
        {
            if (clientSecretTypeOptions.Reader is null)
            {
                continue;
            }
            if (clientSecretTypeOptions.Reader.Implementation is null)
            {
                continue;
            }
            if (clientSecretTypeOptions.Reader.Abstraction is null || !TryGetType(clientSecretTypeOptions.Reader.Abstraction.AssemblyName, clientSecretTypeOptions.Reader.Abstraction.TypeName, out Type? abstractionType))
            {
                if (!TryGetType(services, clientSecretTypeOptions.Name, out abstractionType))
                {
                    continue;
                }
            }
            var clientSecretTypeMetadata = ClientSecretReaderMetadata.Create(clientSecretTypeOptions.Name, abstractionType!, clientSecretTypeOptions.Description);

            if (!TryGetType(clientSecretTypeOptions.Reader.Implementation.AssemblyName, clientSecretTypeOptions.Reader.Implementation.TypeName, out Type? implementationType))
            {
                continue;
            }

            services.SetOAuth20ClientSecretType(clientSecretTypeMetadata, implementationType!);
        }

        return services;
    }

    public static IServiceCollection SetOAuth20ClientSecretType<TAbstraction, TImplementation>(this IServiceCollection services, string clientSecretType, string? description = null)
        where TImplementation : TAbstraction
        where TAbstraction : IClientSecretReader
        => services.SetOAuth20ClientSecretType(clientSecretType, typeof(TAbstraction), typeof(TImplementation), description);

    public static IServiceCollection SetOAuth20ClientSecretType(this IServiceCollection services, string clientSecretType, Type abstraction, Type implementation, string? description = null)
        => services.SetOAuth20ClientSecretType(ClientSecretReaderMetadata.Create(clientSecretType, abstraction, description), implementation);

    public static IServiceCollection SetOAuth20ClientSecretType(this IServiceCollection services, ClientSecretReaderMetadata clientSecretTypeMetadata, Type implementation)
    {
        services.SetOAuth20ClientSecretType(clientSecretTypeMetadata);
        services.AddScoped(clientSecretTypeMetadata.Abstraction, implementation);

        return services;
    }

    public static IServiceCollection SetOAuth20ClientSecretType<TImplementation>(this IServiceCollection services, ClientSecretReaderMetadata clientSecretTypeMetadata)
        where TImplementation : IClientSecretReader
        => services.SetOAuth20ClientSecretType(clientSecretTypeMetadata, typeof(TImplementation));

    private static IServiceCollection SetOAuth20DefaultClientSecretTypes(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;

        services.SetOAuth20DefaultClientSecretType<IAuthorizationHeaderBasicClientSecretReader, DefaultAuthorizationHeaderBasicClientSecretReader>(
            options.ClientSecrets?.AuthorizationHeaderBasicClientSecretTypeName ?? "authorization_header_basic",
            "Authorization Header Basic ClientSecretReader");

        services.SetOAuth20DefaultClientSecretType<IRequestBodyClientCredentialsClientSecretReader, DefaultRequestBodyClientCredentialsClientSecretReader>(
            options.ClientSecrets?.AuthorizationHeaderBasicClientSecretTypeName ?? "request_body_client_credentials",
            "Request Body Client Credentials ClientSecretReader");

        services.SetOAuth20DefaultClientSecretType<ITlsCertificateClientSecretReader, DefaultTlsCertificateClientSecretReader>(
            options.ClientSecrets?.AuthorizationHeaderBasicClientSecretTypeName ?? "tls_certificate",
            "TLS Certificate ClientSecretReader");

        return services;
    }

    private static IServiceCollection SetOAuth20DefaultClientSecretType<TDefaultAbstraction, TDefaultImplementation>(this IServiceCollection services, string clientSecretType, string? defaultDescription = null)
        where TDefaultImplementation : TDefaultAbstraction
        where TDefaultAbstraction : IClientSecretReader
        => services.SetOAuth20DefaultClientSecretType(clientSecretType, typeof(TDefaultAbstraction), typeof(TDefaultImplementation), defaultDescription);

    private static IServiceCollection SetOAuth20DefaultClientSecretType(this IServiceCollection services, string clientSecretType, Type defaultAbstraction, Type defaultImplementation, string? defaultDescription = null)
    {
        services.SetOAuth20ClientSecretType(ClientSecretReaderMetadata.Create(clientSecretType, defaultAbstraction, defaultDescription), defaultImplementation);

        return services;
    }

    private static IServiceCollection SetOAuth20ClientSecretType(this IServiceCollection services, ClientSecretReaderMetadata clientSecretTypeMetadata)
    {
        var clientSecretTypeMetadataCollection = services.BuildServiceProvider().GetRequiredService<IClientSecretReaderMetadataCollection>();

        clientSecretTypeMetadataCollection.ClientSecretReaders[clientSecretTypeMetadata.ClientSecretType] = clientSecretTypeMetadata;

        services.AddSingleton(clientSecretTypeMetadataCollection);

        return services;
    }

    private static bool TryGetType(IServiceCollection services, string clientSecretType, out Type? type)
    {
        var clientSecretReaderMetadataCollection = services.BuildServiceProvider().GetRequiredService<IClientSecretReaderMetadataCollection>();

        if (clientSecretReaderMetadataCollection.ClientSecretReaders.TryGetValue(clientSecretType, out ClientSecretReaderMetadata? clientSecretReaderMetadata))
        {
            type = clientSecretReaderMetadata.Abstraction;
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
