// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace ExtDotNet.OAuth20.Server.Options.Entities;

public class ClientSecretEntityOptions
{
    [Required]
    public string Content { get; set; } = default!;

    public string? Title { get; set; }

    public string? Desription { get; set; }

    [Required]
    public string ClientSecretType { get; set; } = default!;
}
