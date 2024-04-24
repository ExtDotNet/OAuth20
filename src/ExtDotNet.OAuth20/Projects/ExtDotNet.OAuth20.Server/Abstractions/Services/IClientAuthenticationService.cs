// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface IClientAuthenticationService
{
    public Task<Client?> AuthenticateClientAsync(HttpContext httpContext);
}
