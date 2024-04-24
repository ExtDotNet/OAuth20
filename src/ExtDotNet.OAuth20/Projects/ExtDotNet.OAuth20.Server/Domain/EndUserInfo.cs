// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class EndUserInfo : Int32IdEntityBase
{
    public int EndUserId { get; set; }

    public EndUser EndUser { get; set; } = default!;

    public string? Description { get; set; }
}
