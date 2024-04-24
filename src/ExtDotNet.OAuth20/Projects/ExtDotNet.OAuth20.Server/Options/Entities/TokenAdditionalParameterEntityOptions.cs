// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace ExtDotNet.OAuth20.Server.Options.Entities;

public class TokenAdditionalParameterEntityOptions
{
    [Required]
    public string Name { get; set; } = default!;

    [Required]
    public string Value { get; set; } = default!;

    public string? Description { get; set; }
}
