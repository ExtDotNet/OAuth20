// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Flows;

public interface IFlowProvider
{
    public bool TryGetFlowInstanceByGrantTypeName(string grantType, out IFlow? flow);

    public bool TryGetFlowInstanceByResponseTypeName(string responseType, out IFlow? flow);
}
