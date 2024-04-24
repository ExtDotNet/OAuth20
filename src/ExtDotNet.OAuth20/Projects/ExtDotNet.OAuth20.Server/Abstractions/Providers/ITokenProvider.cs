// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.Abstractions.Providers;

public interface ITokenProvider
{
    public Task<TokenType> GetTokenTypeAsync(Client client);

    public Task<string> GetTokenValueAsync(TokenType tokenType, TokenContext tokenContext);
}
