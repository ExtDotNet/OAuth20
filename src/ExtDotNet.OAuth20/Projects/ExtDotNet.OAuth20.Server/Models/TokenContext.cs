// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Models;

public class TokenContext
{
    public IEnumerable<Scope> Scopes { get; set; } = default!;

    public Client Client { get; set; } = default!;

    public DateTimeOffset? CreationDateTime { get; set; }

    public DateTimeOffset? ActivationDateTime { get; set; }

    public DateTimeOffset? ExpirationDateTime { get; set; }

    public long? LifetimeSeconds { get; set; }

    public string? Issuer { get; set; }

    public IEnumerable<string>? Audiences { get; set; }

    public IDictionary<string, string>? AdditionalParameters { get; set; }

    public EndUser? EndUser { get; set; }

    public string? RedirectUri { get; set; } = default!;
}
