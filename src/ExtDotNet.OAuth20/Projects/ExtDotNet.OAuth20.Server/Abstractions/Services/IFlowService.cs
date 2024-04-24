// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IFlowService
{
    public Task<Flow?> GetFlowAsync(string name);

    public Task<Flow?> GetFlowAsync<T>()
        where T : IFlow;

    public Task<Flow?> GetFlowAsync<T>(T implementation)
        where T : IFlow;

    public Task<Flow?> GetFlowAsync(Type type);
}
