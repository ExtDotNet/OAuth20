// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using Microsoft.IdentityModel.Tokens;

namespace ExtDotNet.OAuth20.Server.Abstractions.ServerSigningCredentials;

public interface IServerSigningCredentialsCollection
{
    public IDictionary<string, SigningCredentials> AlgorithmKeySigningCredentials { get; set; }

    public SigningCredentials DefaultSigningCredentials { get; set; }
}
