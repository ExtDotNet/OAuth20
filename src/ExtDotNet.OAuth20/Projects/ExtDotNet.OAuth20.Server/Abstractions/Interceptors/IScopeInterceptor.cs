// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.Interceptors;

/// <summary>
/// Service for intercepting a requested and an issued scopes.
/// </summary>
public interface IScopeInterceptor
{
    /// <summary>
    /// This method is executed when a requested scope is formed.
    /// </summary>
    public Task<string?> OnExecutingAsync(string? requestedScope, Client client, string? state = null);

    /// <summary>
    /// This method is executed when a requested scope is formed.
    /// </summary>
    public Task<IEnumerable<Scope>> OnExecutingAsync(IEnumerable<Scope> requestedScope, Client client, string? state = null);

    /// <summary>
    /// This method is executed when an issued scope is formed.
    /// </summary>
    public Task<IEnumerable<Scope>> OnExecutedAsync(IEnumerable<Scope> issuedScope, Client client, string? state = null);

    /// <summary>
    /// This method is executed when an issued scope is formed.
    /// </summary>
    public Task<string> OnExecutedAsync(string issuedScope, Client client, string? state = null);
}
