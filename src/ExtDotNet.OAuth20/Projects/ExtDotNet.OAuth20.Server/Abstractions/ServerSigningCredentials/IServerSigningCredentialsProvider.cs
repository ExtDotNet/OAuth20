// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using Microsoft.IdentityModel.Tokens;

namespace ExtDotNet.OAuth20.Server.Abstractions.ServerSigningCredentials;

public interface IServerSigningCredentialsProvider
{
    public Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync(IEnumerable<SigningCredentialsAlgorithm> signingCredentialsAlgorithms);

    public Task<SigningCredentials> GetDefaultSigningCredentialsAsync();
}
