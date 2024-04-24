// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;

namespace ExtDotNet.OAuth20.Server.Default.Flows;

public class DefaultFlowProvider : IFlowProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IFlowMetadataCollection _flowMetadataCollection;

    public DefaultFlowProvider(IServiceProvider serviceProvider, IFlowMetadataCollection flowMetadataCollection)
    {
        _serviceProvider = serviceProvider;
        _flowMetadataCollection = flowMetadataCollection;
    }

    public bool TryGetFlowInstanceByGrantTypeName(string grantType, out IFlow? flow)
    {
        FlowMetadata? flowMetadata = _flowMetadataCollection.FlowsWithGrantType[grantType];

        if (flowMetadata is not null)
        {
            flow = (IFlow)_serviceProvider.GetRequiredService(flowMetadata.Abstraction);
            return true;
        }
        else
        {
            flow = null;
            return false;
        }
    }

    public bool TryGetFlowInstanceByResponseTypeName(string responseType, out IFlow? flow)
    {
        FlowMetadata? flowMetadata = _flowMetadataCollection.FlowsWithResponseType[responseType];

        if (flowMetadata is not null)
        {
            flow = (IFlow)_serviceProvider.GetRequiredService(flowMetadata.Abstraction);
            return true;
        }
        else
        {
            flow = null;
            return false;
        }
    }
}
