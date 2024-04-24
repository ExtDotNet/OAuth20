// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IScopeService
{
    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.3"/>фыыф
    /// </summary>
    public Task<ScopeResult> GetServerAllowedScopeAsync(string? clientRequestedScope, Client client, string? state = null);

    public Task<bool> HasEndUserClientScopeGrantedAsync(EndUser endUser, Client client);

    public Task<ScopeResult> GetEndUserClientScopeAsync(string? requestedScope, EndUser endUser, Client client, string? state = null);

    public Task<IEnumerable<Scope>> GetScopeListAsync(string scope);

    public bool ScopesEqual(string scope1, string? scope2);
}
