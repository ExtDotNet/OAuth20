// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class ClientSecretType : Int32IdEntityBase, INamedEntity
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public IEnumerable<ClientSecret>? ClientSecrets { get; set; }
}
