// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.DataSources;

public interface ITokenTypeDataSource
{
    public Task<TokenType?> GetTokenTypeAsync(string name);

    public Task<TokenType?> GetTokenTypeAsync(Client client);

    public Task<IEnumerable<TokenAdditionalParameter>> GetTokenAdditionalParametersAsync(TokenType tokenType);
}
