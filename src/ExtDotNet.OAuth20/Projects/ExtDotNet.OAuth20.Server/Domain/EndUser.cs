// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class EndUser : Int32IdEntityBase
{
    public string Username { get; set; } = default!;

    public string? PasswordHash { get; set; }

    public int? EndUserInfoId { get; set; }

    public EndUserInfo? EndUserInfo { get; set; }
}
