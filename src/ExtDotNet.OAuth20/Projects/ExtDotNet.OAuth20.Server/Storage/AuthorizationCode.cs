// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Storage;

public class AuthorizationCode : EntityBase<int>
{
    public string Value { get; set; } = default!;

    public int EndUserId { get; set; }

    public EndUser EndUser { get; set; } = default!;

    public string RedirectUri { get; set; } = default!;

    public int AuthorizationCodeScopeSetId { get; set; }

    public AuthorizationCodeScopeSet AuthorizationCodeScopeSet { get; set; } = default!;

    public bool IssuedScopeDifferent { get; set; }

    public DateTime IssueDateTime { get; set; }

    public DateTime ActivationDateTime { get; set; }

    public long? ExpiresIn { get; set; }

    public DateTime? ExpirationDateTime { get; set; }

    public bool Issued { get; set; } = false;

    public bool Exchanged { get; set; } = false;

    public bool Expired { get; set; } = false;
}
