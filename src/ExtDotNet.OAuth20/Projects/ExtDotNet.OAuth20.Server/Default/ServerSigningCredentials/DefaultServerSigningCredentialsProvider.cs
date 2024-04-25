// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ServerSigningCredentials;
using ExtDotNet.OAuth20.Server.Domain;
using Microsoft.IdentityModel.Tokens;

namespace ExtDotNet.OAuth20.Server.Default.ServerSigningCredentials;

public class DefaultServerSigningCredentialsProvider(IServerSigningCredentialsCollection serverSigningCredentialsCollection)
    : IServerSigningCredentialsProvider
{
    private readonly IServerSigningCredentialsCollection _serverSigningCredentialsCollection = serverSigningCredentialsCollection ??
        throw new ArgumentNullException(nameof(serverSigningCredentialsCollection));

    public Task<IEnumerable<SigningCredentials>> GetSigningCredentialsAsync(IEnumerable<SigningCredentialsAlgorithm> signingCredentialsAlgorithms)
    {
        var algorithmKeySigningCredentials = _serverSigningCredentialsCollection.AlgorithmKeySigningCredentials;

        List<SigningCredentials> signingCredentialsList = new(signingCredentialsAlgorithms.Count());

        foreach (var signingCredentialsAlgorithm in signingCredentialsAlgorithms)
        {
            if (algorithmKeySigningCredentials.TryGetValue(signingCredentialsAlgorithm.Name, out var signingCredentials))
            {
                signingCredentialsList.Add(signingCredentials);
            }
        }

        return Task.FromResult<IEnumerable<SigningCredentials>>(signingCredentialsList);
    }

    public ValueTask<SigningCredentials> GetDefaultSigningCredentialsAsync() =>
        ValueTask.FromResult(_serverSigningCredentialsCollection.DefaultSigningCredentials);
}
