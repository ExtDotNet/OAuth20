// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Models;

public class AccessTokenResult
{
    public string Value { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string Scope { get; set; } = default!;

    public bool IssuedScopeDifferent { get; set; } = false;

    public string? Username { get; set; } = default!;

    public string ClientId { get; set; } = default!;

    public string? RedirectUri { get; set; } = default!;

    public DateTime IssueDateTime { get; set; }

    public DateTime ActivationDateTime { get; set; }

    public long? ExpiresIn { get; set; } = 3600;

    public DateTime? ExpirationDateTime { get; set; }

    public bool Issued { get; set; } = false;
}
