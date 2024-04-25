// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ClientSecretReaders;
using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Abstractions.Reporitories;
using ExtDotNet.OAuth20.Server.Abstractions.Reporitories.Common;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Domain.Enums;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.ServiceCollections;

public static class IRepositoryServiceCollectionExtensions
{
    public static IServiceCollection SetOAuth20DataRepositories(this IServiceCollection services, IRepositoryContext repositoryContext)
    {
        repositoryContext.SetRepositories(services);

        return services;
    }

    public static IServiceCollection SetOAuth20EntitiesFromOptions(this IServiceCollection services, IRepositoryContext repositoryContext)
    {
        services.SetOAuth20DataRepositories(repositoryContext);

        services.SetOAuth20SigningCredentialsAlgorithmEntitiesFromOptions();
        services.SetOAuth20ClientSecretTypeEntitiesFromOptions();
        services.SetOAuth20ClientSecretTypeEntitiesFromMetadataCollection();
        services.SetOAuth20TokenAdditionalParameterEntitiesFromOptions();
        services.SetOAuth20TokenTypeEntitiesFromOptions();
        services.SetOAuth20TokenTypeEntitiesFromMetadataCollection();
        services.SetOAuth20FlowEntitiesFromOptions();
        services.SetOAuth20FlowEntitiesFromMetadataCollection();
        services.SetOAuth20ResourceEntitiesFromOptions();
        services.SetOAuth20ClientTypeEntitiesFromEnum();
        services.SetOAuth20ClientProfileEntitiesFromEnum();
        services.SetOAuth20ClientEntitiesFromOptions();
        services.SetOAuth20EndUserEntitiesFromOptions();

        return services;
    }

    public static IServiceCollection SetOAuth20SigningCredentialsAlgorithmEntitiesFromOptions(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;
        var repository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<SigningCredentialsAlgorithm>>();

        var scaOptionsList = options.Entities?.SigningCredentialsAlgorithms;

        if (scaOptionsList?.Any() is not true) return services;

        foreach (var scaOptions in scaOptionsList)
        {
            var existingEntity = repository.GetByNameAsync(scaOptions.Name).GetAwaiter().GetResult();

            if (existingEntity is null)
            {
                SigningCredentialsAlgorithm scaEntity = new()
                {
                    Name = scaOptions.Name,
                    Description = scaOptions.Description,
                };

                repository.AddAsync(scaEntity).GetAwaiter().GetResult();
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20ClientSecretTypeEntitiesFromOptions(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;
        var repository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<ClientSecretType>>();

        var cstOptionsList = options.Entities?.ClientSecretTypes;

        if (cstOptionsList?.Any() is not true) return services;

        foreach (var cstOptions in cstOptionsList)
        {
            var existingCstEntity = repository.GetByNameAsync(cstOptions.Name).GetAwaiter().GetResult();

            if (existingCstEntity is null)
            {
                ClientSecretType cstEntity = new()
                {
                    Name = cstOptions.Name,
                    Description = cstOptions.Description,
                };

                repository.AddAsync(cstEntity).GetAwaiter().GetResult();
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20ClientSecretTypeEntitiesFromMetadataCollection(this IServiceCollection services)
    {
        var metadataCollection = services.BuildServiceProvider().GetRequiredService<IClientSecretReaderMetadataCollection>();
        var repository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<ClientSecretType>>();

        var cstMetadataList = metadataCollection.ClientSecretReaders.Values;

        if (cstMetadataList?.Any() is not true) return services;

        foreach (var cstMetadata in cstMetadataList)
        {
            var existingCstEntity = repository.GetByNameAsync(cstMetadata.ClientSecretType).GetAwaiter().GetResult();

            if (existingCstEntity is null)
            {
                ClientSecretType cstEntity = new()
                {
                    Name = cstMetadata.ClientSecretType,
                    Description = cstMetadata.Description,
                };

                repository.AddAsync(cstEntity).GetAwaiter().GetResult();
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20TokenAdditionalParameterEntitiesFromOptions(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;
        var repository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<TokenAdditionalParameter>>();

        var tapOptionsList = options.Entities?.TokenAdditionalParameters;

        if (tapOptionsList?.Any() is not true) return services;

        foreach (var tapOptions in tapOptionsList)
        {
            var existingTapEntity = repository.GetByNameAsync(tapOptions.Name).GetAwaiter().GetResult();

            if (existingTapEntity is null)
            {
                TokenAdditionalParameter tapEntity = new()
                {
                    Name = tapOptions.Name,
                    Value = tapOptions.Value,
                    Description = tapOptions.Description,
                };

                repository.AddAsync(tapEntity).GetAwaiter().GetResult();
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20TokenTypeEntitiesFromOptions(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;
        var ttRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<TokenType>>();
        var tapRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<TokenAdditionalParameter>>();
        var ttTapRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdRepository<TokenTypeTokenAdditionalParameter>>();

        var ttOptionsList = options.Entities?.TokenTypes;

        if (ttOptionsList?.Any() is not true) return services;

        foreach (var ttOptions in ttOptionsList)
        {
            var existingTtEntity = ttRepository.GetByNameAsync(ttOptions.Name).GetAwaiter().GetResult();

            if (existingTtEntity is null)
            {
                TokenType ttEntity = new()
                {
                    Name = ttOptions.Name,
                    Description = ttOptions.Description,
                };

                int ttEntityId = ttRepository.AddAsync(ttEntity).GetAwaiter().GetResult();

                if (ttOptions.AdditionalParameters?.Any() is true)
                {
                    foreach (var tapKey in ttOptions.AdditionalParameters)
                    {
                        var tapEntity = tapRepository.GetByNameAsync(tapKey).GetAwaiter().GetResult();

                        if (tapEntity is null) throw new ArgumentException($"{nameof(tapKey)}:{tapKey}"); // TODO: detailed error

                        TokenTypeTokenAdditionalParameter ttTapEntity = new()
                        {
                            TokenTypeId = ttEntityId,
                            TokenAdditionalParameterId = tapEntity.Id
                        };

                        ttTapRepository.AddAsync(ttTapEntity).GetAwaiter().GetResult();
                    }
                }
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20TokenTypeEntitiesFromMetadataCollection(this IServiceCollection services)
    {
        var metadataCollection = services.BuildServiceProvider().GetRequiredService<ITokenBuilderMetadataCollection>();
        var ttRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<TokenType>>();
        var tapRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<TokenAdditionalParameter>>();
        var ttTapRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdRepository<TokenTypeTokenAdditionalParameter>>();

        var tbMetadataList = metadataCollection.TokenBuilders.Values;

        if (tbMetadataList?.Any() is not true) return services;

        foreach (var tbMetadata in tbMetadataList)
        {
            var existingTtEntity = ttRepository.GetByNameAsync(tbMetadata.TokenType).GetAwaiter().GetResult();

            if (existingTtEntity is null)
            {
                TokenType ttEntity = new()
                {
                    Name = tbMetadata.TokenType,
                    Description = tbMetadata.Description,
                };

                int ttEntityId = ttRepository.AddAsync(ttEntity).GetAwaiter().GetResult();

                if (tbMetadata.AdditionalParameters?.Any() is true)
                {
                    foreach (var tapKey in tbMetadata.AdditionalParameters.Values)
                    {
                        var tapEntity = tapRepository.GetByNameAsync(tapKey).GetAwaiter().GetResult();

                        if (tapEntity is null) throw new ArgumentException($"{nameof(tapKey)}:{tapKey}"); // TODO: detailed error

                        TokenTypeTokenAdditionalParameter ttTapEntity = new()
                        {
                            TokenTypeId = ttEntityId,
                            TokenAdditionalParameterId = tapEntity.Id
                        };

                        ttTapRepository.AddAsync(ttTapEntity).GetAwaiter().GetResult();
                    }
                }
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20FlowEntitiesFromOptions(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;
        var repository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<Flow>>();

        var flowOptionsList = options.Entities?.Flows;

        if (flowOptionsList?.Any() is not true) return services;

        foreach (var flowOptions in flowOptionsList)
        {
            var existingFlowEntity = repository.GetByNameAsync(flowOptions.Name).GetAwaiter().GetResult();

            if (existingFlowEntity is null)
            {
                Flow flowEntity = new()
                {
                    Name = flowOptions.Name,
                    Description = flowOptions.Description,
                    GrantTypeName = flowOptions.GrantTypeName,
                    ResponseTypeName = flowOptions.ResponseTypeName,
                };

                repository.AddAsync(flowEntity).GetAwaiter().GetResult();
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20FlowEntitiesFromMetadataCollection(this IServiceCollection services)
    {
        var repository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<Flow>>();
        var flowMetadataCollection = services.BuildServiceProvider().GetRequiredService<IFlowMetadataCollection>();

        var flowMetadataList = flowMetadataCollection.Flows.Values;

        foreach (var flowMetadata in flowMetadataList)
        {
            var existingFlowEntity = repository.GetByNameAsync(flowMetadata.Name).GetAwaiter().GetResult();

            if (existingFlowEntity is null)
            {
                Flow flowEntity = new()
                {
                    Name = flowMetadata.Name,
                    Description = flowMetadata.Description,
                    GrantTypeName = flowMetadata.GrantTypeName,
                    ResponseTypeName = flowMetadata.ResponseTypeName,
                };

                repository.AddAsync(flowEntity).GetAwaiter().GetResult();
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20ResourceEntitiesFromOptions(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;
        var resourceRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<Resource>>();
        var scopeRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<Scope>>();
        var scaRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<SigningCredentialsAlgorithm>>();
        var rScaRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdRepository<ResourceSigningCredentialsAlgorithm>>();

        var resourceOptionsList = options.Entities?.Resources;

        if (resourceOptionsList?.Any() is not true) return services;

        foreach (var resourceOptions in resourceOptionsList)
        {
            var existingResourceEntity = resourceRepository.GetByNameAsync(resourceOptions.Name).GetAwaiter().GetResult();

            if (existingResourceEntity is null)
            {
                Resource resourceEntity = new()
                {
                    Name = resourceOptions.Name,
                    Description = resourceOptions.Description,
                };

                int resourceEntityId = resourceRepository.AddAsync(resourceEntity).GetAwaiter().GetResult();

                if (resourceOptions.Scopes?.Any() is true)
                {
                    foreach (var scopeOptions in resourceOptions.Scopes)
                    {
                        var scopeExistingEntity = scopeRepository.GetByNameAsync(scopeOptions.Name).GetAwaiter().GetResult();

                        if (scopeExistingEntity is not null)
                        {
                            throw new ArgumentException($"{nameof(scopeExistingEntity.Name)}:{scopeExistingEntity.Name}"); // TODO: detailed error
                        }

                        Scope scopeEntity = new()
                        {
                            Name = scopeOptions.Name,
                            Description = scopeOptions.Description,
                            ResourceId = resourceEntityId,
                        };

                        scopeRepository.AddAsync(scopeEntity).GetAwaiter().GetResult();
                    }
                }

                if (resourceOptions.SigningCredentialsAlgorithms?.Any() is true)
                {
                    foreach (var scaName in resourceOptions.SigningCredentialsAlgorithms)
                    {
                        var scaEntity = scaRepository.GetByNameAsync(scaName).GetAwaiter().GetResult();

                        if (scaEntity is null) throw new ArgumentException($"{nameof(scaName)}:{scaName}"); // TODO: detailed error

                        ResourceSigningCredentialsAlgorithm rScaEntity = new()
                        {
                            ResourceId = resourceEntityId,
                            SigningCredentialsAlgorithmId = scaEntity.Id,
                        };

                        rScaRepository.AddAsync(rScaEntity).GetAwaiter().GetResult();
                    }
                }
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20ClientTypeEntitiesFromEnum(this IServiceCollection services)
    {
        var clientTypeRepository = services.BuildServiceProvider().GetRequiredService<IClientTypeRepository>();
        var clientTypeEnumValueList = Enum.GetValues<Domain.Enums.ClientType>().Where(x => x != Domain.Enums.ClientType.Undefined);

        if (clientTypeEnumValueList?.Any() is not true) return services;

        foreach (var clientTypeEnumValue in clientTypeEnumValueList)
        {
            var existingClientTypeEntity = clientTypeRepository.GetByIdAsync(clientTypeEnumValue).GetAwaiter().GetResult();

            if (existingClientTypeEntity is null)
            {
                Domain.ClientType clientType = new()
                {
                    Id = clientTypeEnumValue,
                    Name = clientTypeEnumValue.ToString(),
                    Description = clientTypeEnumValue.GetDescriptionAttributeValue()
                };

                clientTypeRepository.AddAsync(clientType).GetAwaiter().GetResult();
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20ClientProfileEntitiesFromEnum(this IServiceCollection services)
    {
        var clientProfileRepository = services.BuildServiceProvider().GetRequiredService<IClientProfileRepository>();
        var clientProfileEnumValueList = Enum.GetValues<Domain.Enums.ClientProfile>().Where(x => x != Domain.Enums.ClientProfile.Undefined);

        if (clientProfileEnumValueList?.Any() is not true) return services;

        foreach (var clientProfileEnumValue in clientProfileEnumValueList)
        {
            var existingClientProfileEntity = clientProfileRepository.GetByIdAsync(clientProfileEnumValue).GetAwaiter().GetResult();

            if (existingClientProfileEntity is null)
            {
                Domain.ClientProfile clientProfile = new()
                {
                    Id = clientProfileEnumValue,
                    Name = clientProfileEnumValue.ToString(),
                    Description = clientProfileEnumValue.GetDescriptionAttributeValue()
                };

                clientProfileRepository.AddAsync(clientProfile).GetAwaiter().GetResult();
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20ClientEntitiesFromOptions(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;
        var clientRepository = services.BuildServiceProvider().GetRequiredService<IClientRepository>();
        var clientTypeRepository = services.BuildServiceProvider().GetRequiredService<INamedRepository<Domain.ClientType, Domain.Enums.ClientType>>();
        var clientProfileRepository = services.BuildServiceProvider().GetRequiredService<INamedRepository<Domain.ClientProfile, Domain.Enums.ClientProfile>>();
        var clientRedirectionEndpointRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdRepository<ClientRedirectionEndpoint>>();
        var tokenTypeRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<TokenType>>();
        var scopeRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<Scope>>();
        var clientScopeRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdRepository<ClientScope>>();
        var flowRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<Flow>>();
        var clientFlowRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdRepository<ClientFlow>>();
        var clientSecretRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdRepository<ClientSecret>>();
        var clientSecretTypeRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdNamedRepository<ClientSecretType>>();

        var clientOptionsList = options.Entities?.Clients;

        if (clientOptionsList?.Any() is not true) return services;

        foreach (var clientOptions in clientOptionsList)
        {
            var existingClientEntity = clientRepository.GetByClientIdAsync(clientOptions.ClientId).GetAwaiter().GetResult();

            if (existingClientEntity is null)
            {
                Client clientEntity = new()
                {
                    ClientId = clientOptions.ClientId,
                    LoginEndpoint = clientOptions.LoginEndpoint,
                    PermissionsEndpoint = clientOptions.PermissionsEndpoint,
                    TokenExpirationSeconds = clientOptions.TokenExpirationSeconds,
                    EndUserPermissionsRequired = clientOptions.EndUserPermissionsRequired,
                };

                var clientType = clientTypeRepository.GetByNameAsync(clientOptions.ClientType).GetAwaiter().GetResult();
                if (clientType is null) throw new ArgumentException($"{nameof(clientType)}:{clientType}"); // TODO: detailed error
                clientEntity.ClientTypeId = clientType.Id;

                var clientProfile = clientProfileRepository.GetByNameAsync(clientOptions.ClientProfile).GetAwaiter().GetResult();
                if (clientProfile is null) throw new ArgumentException($"{nameof(clientProfile)}:{clientProfile}"); // TODO: detailed error
                clientEntity.ClientProfileId = clientProfile.Id;

                if (clientOptions.TokenType is not null)
                {
                    var tokenType = tokenTypeRepository.GetByNameAsync(clientOptions.TokenType).GetAwaiter().GetResult();
                    if (tokenType is null) throw new ArgumentException($"{nameof(tokenType)}:{tokenType}"); // TODO: detailed error
                    clientEntity.TokenTypeId = tokenType.Id;
                }

                int clientEntityId = clientRepository.AddAsync(clientEntity).GetAwaiter().GetResult();

                if (clientOptions.RedirectionEndpoints?.Any() is true)
                {
                    foreach (var redirectionEndpointValue in clientOptions.RedirectionEndpoints)
                    {
                        ClientRedirectionEndpoint clientRedirectionEndpoint = new()
                        {
                            Value = redirectionEndpointValue,
                            ClientId = clientEntityId,
                        };

                        clientRedirectionEndpointRepository.AddAsync(clientRedirectionEndpoint).GetAwaiter().GetResult();
                    }
                }

                if (clientOptions.Scopes?.Any() is true)
                {
                    foreach (var scopeName in clientOptions.Scopes)
                    {
                        var scopeEntity = scopeRepository.GetByNameAsync(scopeName).GetAwaiter().GetResult();
                        if (scopeEntity is null) throw new ArgumentException($"{nameof(scopeEntity)}:{scopeEntity}"); // TODO: detailed error

                        ClientScope clientScopeEntity = new()
                        {
                            ClientId = clientEntityId,
                            ScopeId = scopeEntity.Id
                        };

                        clientScopeRepository.AddAsync(clientScopeEntity).GetAwaiter().GetResult();
                    }
                }

                if (clientOptions.Flows?.Any() is true)
                {
                    foreach (var flowName in clientOptions.Flows)
                    {
                        var flowEntity = flowRepository.GetByNameAsync(flowName).GetAwaiter().GetResult();
                        if (flowEntity is null) throw new ArgumentException($"{nameof(flowEntity)}:{flowEntity}"); // TODO: detailed error

                        ClientFlow clientFlowEntity = new()
                        {
                            ClientId = clientEntityId,
                            FlowId = flowEntity.Id
                        };

                        clientFlowRepository.AddAsync(clientFlowEntity).GetAwaiter().GetResult();
                    }
                }

                if (clientOptions.ClientSecrets?.Any() is true)
                {
                    foreach (var clientSecretOptions in clientOptions.ClientSecrets)
                    {
                        ClientSecret clientSecret = new()
                        {
                            ClientId = clientEntityId,
                            Title = clientSecretOptions.Title,
                            Content = clientSecretOptions.Content,
                            Desription = clientSecretOptions.Desription
                        };

                        var clientSecretType = clientSecretTypeRepository.GetByNameAsync(clientSecretOptions.ClientSecretType).GetAwaiter().GetResult();
                        if (clientSecretType is null) throw new ArgumentException($"{nameof(clientSecretType)}:{clientSecretType}"); // TODO: detailed error
                        clientSecret.ClientSecretTypeId = clientSecretType.Id;

                        clientSecretRepository.AddAsync(clientSecret).GetAwaiter().GetResult();
                    }
                }
            }
        }

        return services;
    }

    public static IServiceCollection SetOAuth20EndUserEntitiesFromOptions(this IServiceCollection services)
    {
        var options = services.BuildServiceProvider().GetRequiredService<IOptions<OAuth20ServerOptions>>().Value;
        var endUserRepository = services.BuildServiceProvider().GetRequiredService<IEndUserRepository>();
        var endUserInfoRepository = services.BuildServiceProvider().GetRequiredService<IInt32IdRepository<EndUserInfo>>();
        var passwordHashingService = services.BuildServiceProvider().GetRequiredService<IPasswordHashingService>();

        var endUserOptionsList = options.Entities?.EndUsers;

        if (endUserOptionsList?.Any() is not true) return services;

        foreach (var endUserOptions in endUserOptionsList)
        {
            var existingEndUserEntity = endUserRepository.GetByUsernameAsync(endUserOptions.Username).GetAwaiter().GetResult();

            if (existingEndUserEntity is null)
            {
                EndUser endUserEntity = new()
                {
                    Username = endUserOptions.Username,
                    PasswordHash = passwordHashingService.GetPasswordHashAsync(endUserOptions.Password).GetAwaiter().GetResult(),
                };

                int endUserEntityId = endUserRepository.AddAsync(endUserEntity).GetAwaiter().GetResult();

                EndUserInfo endUserInfo = new()
                {
                    EndUserId = endUserEntityId,
                    Description = endUserOptions.Description,
                };

                int endUserInfoEntityId = endUserInfoRepository.AddAsync(endUserInfo).GetAwaiter().GetResult();

                endUserEntity.EndUserInfoId = endUserInfoEntityId;
                endUserEntity.EndUserInfo = endUserInfo;

                endUserRepository.UpdateAsync(endUserEntity).GetAwaiter().GetResult();
            }
        }

        return services;
    }
}
