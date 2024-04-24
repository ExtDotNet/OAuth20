// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class ClientRedirectionEndpoint : Int32IdEntityBase
{
    public string Value { get; set; } = default!;

    public int ClientId { get; set; }

    public Client Client { get; set; } = default!;
}
