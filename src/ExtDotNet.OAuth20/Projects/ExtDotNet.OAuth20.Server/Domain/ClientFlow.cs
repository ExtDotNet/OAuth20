// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class ClientFlow : Int32IdEntityBase
{
    public int ClientId { get; set; }

    public Client Client { get; set; } = default!;

    public int FlowId { get; set; }

    public Flow Flow { get; set; } = default!;
}
