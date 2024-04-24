// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Storage;

public class AccessToken : EntityBase<int>
{
    public string Value { get; set; } = default!;

    public int TokenTypeId { get; set; }

    public TokenType TokenType { get; set; } = default!;

    public int? EndUserId { get; set; }

    public EndUser? EndUser { get; set; }

    public string RedirectUri { get; set; } = default!;

    public int AccessTokenScopeSetId { get; set; }

    public AccessTokenScopeSet AccessTokenScopeSet { get; set; } = default!;

    public bool IssuedScopeDifferent { get; set; }

    public DateTime IssueDateTime { get; set; }

    public DateTime ActivationDateTime { get; set; }

    public long? ExpiresIn { get; set; }

    public DateTime? ExpirationDateTime { get; set; }

    public bool Issued { get; set; } = false;

    public bool Expired { get; set; } = false;
}
