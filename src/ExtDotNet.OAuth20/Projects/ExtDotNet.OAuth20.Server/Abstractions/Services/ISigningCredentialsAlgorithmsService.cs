// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface ISigningCredentialsAlgorithmsService
{
    public Task<IEnumerable<SigningCredentialsAlgorithm>> GetSigningCredentialsAlgorithmsForScopesAsync(IEnumerable<Scope> scopes);
}
