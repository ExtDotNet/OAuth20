// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Flows;

public interface IFlowMetadataCollection
{
    public IDictionary<string, FlowMetadata> Flows { get; set; }

    public IDictionary<string, FlowMetadata> FlowsWithGrantType { get; set; }

    public IDictionary<string, FlowMetadata> FlowsWithResponseType { get; set; }
}
