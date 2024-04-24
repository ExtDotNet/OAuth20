// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;

namespace ExtDotNet.OAuth20.Server.Default.TokenBuilders;

public class DefaultTokenBuilderProvider : ITokenBuilderProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITokenBuilderMetadataCollection _tokenBuilderMetadataCollection;

    public DefaultTokenBuilderProvider(IServiceProvider serviceProvider, ITokenBuilderMetadataCollection tokenBuilderMetadataCollection)
    {
        _serviceProvider = serviceProvider;
        _tokenBuilderMetadataCollection = tokenBuilderMetadataCollection;
    }

    public bool TryGetTokenBuilderInstanceByType(string type, out ITokenBuilder? tokenBuilder)
    {
        TokenBuilderMetadata? tokenBuilderMetadata = _tokenBuilderMetadataCollection.TokenBuilders[type];

        if (tokenBuilderMetadata is not null)
        {
            tokenBuilder = (ITokenBuilder)_serviceProvider.GetRequiredService(tokenBuilderMetadata.Abstraction);
            return true;
        }
        else
        {
            tokenBuilder = null;
            return false;
        }
    }
}
