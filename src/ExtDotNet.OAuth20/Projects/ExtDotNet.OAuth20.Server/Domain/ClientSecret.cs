// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class ClientSecret : Int32IdEntityBase
{
    public string Content { get; set; } = default!;

    public string? Title { get; set; }

    public string? Desription { get; set; }

    public int ClientId { get; set; }

    public Client Client { get; set; } = default!;

    public int ClientSecretTypeId { get; set; }

    public ClientSecretType ClientSecretType { get; set; } = default!;
}
