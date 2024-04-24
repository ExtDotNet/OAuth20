// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace ExtDotNet.OAuth20.Server.Options.Errors;

public class ErrorOptions
{
    [Required]
    public string Code { get; set; } = default!;

    public string? Description { get; set; }

    public string? Uri { get; set; }
}
