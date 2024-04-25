// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;
using ExtDotNet.OAuth20.Server.Abstractions.Flows;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultFlowService : IFlowService
{
    public DefaultFlowService(IFlowDataSource flowDataSource, IOptions<OAuth20ServerOptions> options)
    {
        FlowDataSource = flowDataSource;
        Options = options;
    }

    protected IFlowDataSource FlowDataSource { get; set; }

    protected IOptions<OAuth20ServerOptions> Options { get; }

    public async Task<Flow?> GetFlowAsync(string name)
    {
        return await FlowDataSource.GetFlowAsync(name).ConfigureAwait(false);
    }

    public virtual async Task<Flow?> GetFlowAsync<T>()
        where T : IFlow
        => await GetFlowAsync(typeof(T)).ConfigureAwait(false);

    public virtual async Task<Flow?> GetFlowAsync<T>(T implementation)
        where T : IFlow
    {
        string? flowName = implementation switch
        {
            IAuthorizationCodeFlow => Options.Value.Flows?.AuthorizationCodeFlowName ?? "authorization_code",
            IImplicitFlow => Options.Value.Flows?.ImplicitFlowName ?? "implicit",
            IClientCredentialsFlow => Options.Value.Flows?.ClientCredentialsFlowName ?? "client_credentials",
            IResourceOwnerPasswordCredentialsFlow => Options.Value.Flows?.ResourceOwnerPasswordCredentialsFlowName ?? "password",
            IRefreshTokenFlow => Options.Value.Flows?.RefreshTokenFlowName ?? "refresh_token",
            _ => null
        };

        if (flowName is not null)
        {
            return await FlowDataSource.GetFlowAsync(flowName).ConfigureAwait(false);
        }
        else
        {
            return null;
        }
    }

    public virtual async Task<Flow?> GetFlowAsync(Type type)
    {
        string? flowName;

        if (type.IsAssignableTo(typeof(IAuthorizationCodeFlow))) flowName = Options.Value.Flows?.AuthorizationCodeFlowName ?? "authorization_code";
        else if (type.IsAssignableTo(typeof(IImplicitFlow))) flowName = Options.Value.Flows?.ImplicitFlowName ?? "implicit";
        else if (type.IsAssignableTo(typeof(IClientCredentialsFlow))) flowName = Options.Value.Flows?.ClientCredentialsFlowName ?? "client_credentials";
        else if (type.IsAssignableTo(typeof(IResourceOwnerPasswordCredentialsFlow))) flowName = Options.Value.Flows?.ResourceOwnerPasswordCredentialsFlowName ?? "password";
        else if (type.IsAssignableTo(typeof(IRefreshTokenFlow))) flowName = Options.Value.Flows?.ResourceOwnerPasswordCredentialsFlowName ?? "refresh_token";
        else flowName = null;

        if (flowName is not null)
        {
            return await FlowDataSource.GetFlowAsync(flowName).ConfigureAwait(false);
        }
        else
        {
            return null;
        }
    }
}
