// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;

using ExtDotNet.OAuth20.Server.Models.Flows;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IPermissionsService
{
    public Task<bool> RedirectToPermissionsRequiredAsync(EndUser endUser, Client client);

    public Task AddPermissionsRequestAsync(ScopeResult scopeResult, EndUser endUser, Client client);

    public Task<ScopeResult?> GetPermissionsRequestAsync(EndUser endUser, Client client);

    public Task AddPermissionsResultAsync(ScopeResult scopeResult, EndUser endUser, Client client);

    public Task<ScopeResult?> GetPermissionsResultAsync(EndUser endUser, Client client);

    public Task<bool> EndUserPermissionsRequiredAsync(Client client);

    public Task<IResult> RedirectToPermissionsAsync(FlowArguments flowArgs, Client client, string? state = null);
}
