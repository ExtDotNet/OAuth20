// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace ExtDotNet.OAuth20.Server.Options.Entities;

public class ClientEntityOptions : EntityOptionsBase
{
    [Required]
    public string ClientId { get; set; } = default!;

    public ClientSecretEntityOptions[]? ClientSecrets { get; set; }

    [Required]
    public string ClientType { get; set; } = default!;

    [Required]
    public string ClientProfile { get; set; } = default!;

    public string[]? RedirectionEndpoints { get; set; }

    public string? LoginEndpoint { get; set; }

    public string? PermissionsEndpoint { get; set; }

    public string? TokenType { get; set; }

    public int? TokenExpirationSeconds { get; set; }

    public bool? EndUserPermissionsRequired { get; set; }

    public string[]? Scopes { get; set; }

    public string[]? Flows { get; set; }
}
