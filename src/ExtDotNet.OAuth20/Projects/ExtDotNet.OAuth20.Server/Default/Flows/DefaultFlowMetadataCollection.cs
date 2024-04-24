// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using System.Collections.Concurrent;

namespace ExtDotNet.OAuth20.Server.Default.Flows;

public class DefaultFlowMetadataCollection : IFlowMetadataCollection
{
    public IDictionary<string, FlowMetadata> Flows { get; set; } = new ConcurrentDictionary<string, FlowMetadata>();

    public IDictionary<string, FlowMetadata> FlowsWithGrantType { get; set; } = new ConcurrentDictionary<string, FlowMetadata>();

    public IDictionary<string, FlowMetadata> FlowsWithResponseType { get; set; } = new ConcurrentDictionary<string, FlowMetadata>();
}
