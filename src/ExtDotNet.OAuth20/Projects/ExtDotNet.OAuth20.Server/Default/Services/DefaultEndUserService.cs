// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.DataSources;
using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Authorize;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using Microsoft.AspNetCore.Authentication;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultEndUserService(
    IHttpContextAccessor httpContextAccessor,
    IEndUserDataSource endUserDataSource,
    IPasswordHashingService passwordHashingService) : IEndUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    private readonly IEndUserDataSource _endUserDataSource = endUserDataSource ?? throw new ArgumentNullException(nameof(endUserDataSource));
    private readonly IPasswordHashingService _passwordHashingService = passwordHashingService ?? throw new ArgumentNullException(nameof(passwordHashingService));

    public async Task<EndUser?> GetCurrentEndUserAsync(string? state = null)
    {
        var authenticationResult = _httpContextAccessor.HttpContext?.AuthenticateAsync().GetAwaiter().GetResult();

        string? username = authenticationResult?.Principal?.Identity?.Name;

        if (username is null) throw new AccessDeniedException($"It isn't specified the {username} (Probably the user isn't authenticated).", state);

        return await GetEndUserAsync(username).ConfigureAwait(false);
    }

    public async Task<EndUser?> GetEndUserAsync(string username) =>
        await _endUserDataSource.GetEndUserAsync(username).ConfigureAwait(false);

    public async Task<EndUser?> GetEndUserAsync(string username, string password)
    {
        string? passwordHash = await _passwordHashingService.GetPasswordHashAsync(password).ConfigureAwait(false);

        return await _endUserDataSource.GetEndUserAsync(username, passwordHash).ConfigureAwait(false);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var authenticationResult = await (_httpContextAccessor.HttpContext?.AuthenticateAsync() ?? Task.FromResult(AuthenticateResult.NoResult()))
            .ConfigureAwait(false);

        return authenticationResult.Succeeded;
    }
}
