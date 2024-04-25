// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.TokenBuilders;

public class DefaultTokenBuilderSelector(ITokenBuilderProvider tokenBuilderProvider) : ITokenBuilderSelector
{
    private readonly ITokenBuilderProvider _tokenBuilderProvider = tokenBuilderProvider ?? throw new ArgumentNullException(nameof(tokenBuilderProvider));

    public bool TryGetTokenBuilder(string type, out ITokenBuilder? tokenBuilder)
    {
        if (_tokenBuilderProvider.TryGetTokenBuilderInstanceByType(type, out ITokenBuilder? tokenBuilderInstance))
        {
            tokenBuilder = tokenBuilderInstance!;
            return true;
        }
        else
        {
            tokenBuilder = null;
            return false;
        }
    }
}
