// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Flows;

public abstract class AuthorizeResultBase : IResult
{
    protected AuthorizeResultBase(string? state = null)
    {
        State = state;
    }

    public string? State { get; set; }

    public abstract Task ExecuteAsync(HttpContext httpContext);
}
