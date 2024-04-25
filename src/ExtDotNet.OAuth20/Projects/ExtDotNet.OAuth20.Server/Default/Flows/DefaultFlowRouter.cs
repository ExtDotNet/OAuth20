// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Flows;

public class DefaultFlowRouter(IFlowProvider flowProvider) : IFlowRouter
{
    private readonly IFlowProvider _flowProvider = flowProvider ?? throw new ArgumentNullException(nameof(flowProvider));

    public bool TryGetAuthorizeFlow(string responseType, out IAuthorizeFlow? authorizeFlow)
    {
        if (_flowProvider.TryGetFlowInstanceByResponseTypeName(responseType, out IFlow? flow))
        {
            authorizeFlow = (IAuthorizeFlow)flow!;
            return true;
        }
        else
        {
            authorizeFlow = null;
            return false;
        }
    }

    public bool TryGetTokenFlow(string grantType, out ITokenFlow? tokenFlow)
    {
        if (_flowProvider.TryGetFlowInstanceByGrantTypeName(grantType, out IFlow? flow))
        {
            tokenFlow = (ITokenFlow)flow!;
            return true;
        }
        else
        {
            tokenFlow = null;
            return false;
        }
    }
}
