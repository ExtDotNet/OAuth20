// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Flows;

public interface IFlowRouter
{
    public bool TryGetAuthorizeFlow(string responseType, out IAuthorizeFlow? authorizeFlow);

    public bool TryGetTokenFlow(string grantType, out ITokenFlow? tokenFlow);
}
