// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Flows;

public abstract class TokenResultBase : IResult
{
    protected TokenResultBase(
        string accessToken,
        string tokenType,
        long? expiresIn = null,
        string? scope = null,
        IDictionary<string, string?>? additionalParameters = null)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
        ExpiresIn = expiresIn;
        Scope = scope;
        AdditionalParameters = additionalParameters;
    }

    public string AccessToken { get; set; } = default!;

    public string TokenType { get; set; } = default!;

    public long? ExpiresIn { get; set; }

    public string? Scope { get; set; }

    public IDictionary<string, string?>? AdditionalParameters { get; set; }

    public abstract Task ExecuteAsync(HttpContext httpContext);
}
