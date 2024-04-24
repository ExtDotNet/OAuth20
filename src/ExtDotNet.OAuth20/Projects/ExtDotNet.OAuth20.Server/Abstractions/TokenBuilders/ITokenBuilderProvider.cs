// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;

public interface ITokenBuilderProvider
{
    public bool TryGetTokenBuilderInstanceByType(string type, out ITokenBuilder? tokenBuilder);
}
