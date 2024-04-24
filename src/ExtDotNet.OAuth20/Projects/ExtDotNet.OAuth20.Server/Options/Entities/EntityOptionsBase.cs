// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.Entities;

public abstract class EntityOptionsBase
{
    public bool CreateIfNotExists { get; set; } = true;

    public bool UpdateIfExists { get; set; } = false;
}
