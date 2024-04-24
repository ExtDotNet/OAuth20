// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.ServerSigningCredentials;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;

namespace ExtDotNet.OAuth20.Server.Default.ServerSigningCredentials;

public class DefaultServerSigningCredentialsCollection : IServerSigningCredentialsCollection
{
    public IDictionary<string, SigningCredentials> AlgorithmKeySigningCredentials { get; set; } = new ConcurrentDictionary<string, SigningCredentials>();

    public SigningCredentials DefaultSigningCredentials { get; set; } = default!;
}
