// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace ExtDotNet.OAuth20.Server.Options.Entities;

public class FlowEntityOptions
{
    [Required]
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public virtual string? GrantTypeName { get; set; }

    public virtual string? ResponseTypeName { get; set; }
}
