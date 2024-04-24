// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Models.Flows;

public class FlowArguments
{
    public HttpRequest HttpRequest { get; set; } = default!;

    public Dictionary<string, string> Values { get; set; } = default!;

    public Dictionary<string, string> Headers { get; set; } = default!;
}
