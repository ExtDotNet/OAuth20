// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace ExtDotNet.OAuth20.Server.Options.Endpoints;

/// <summary>
/// TODO: more advanced business validation.
/// </summary>
public class EndpointOptions
{
    public string? Description { get; set; }

    [Required]
    public string Route { get; set; } = default!;

    public EndpointTypeOptions? Abstraction { get; set; }

    public EndpointTypeOptions? Implementation { get; set; }
}
