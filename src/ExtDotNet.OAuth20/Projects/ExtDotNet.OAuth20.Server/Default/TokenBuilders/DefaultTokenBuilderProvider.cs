// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.TokenBuilders;

public class DefaultTokenBuilderProvider(IServiceProvider serviceProvider, ITokenBuilderMetadataCollection tokenBuilderMetadataCollection) : ITokenBuilderProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    private readonly ITokenBuilderMetadataCollection _tokenBuilderMetadataCollection = tokenBuilderMetadataCollection ??
        throw new ArgumentNullException(nameof(tokenBuilderMetadataCollection));

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
