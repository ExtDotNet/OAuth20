﻿// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace ExtDotNet.OAuth20.Server.Options.Flows;

public class FlowTypeOptions
{
    [Required]
    public string AssemblyName { get; set; } = default!;

    [Required]
    public string TypeName { get; set; } = default!;
}
