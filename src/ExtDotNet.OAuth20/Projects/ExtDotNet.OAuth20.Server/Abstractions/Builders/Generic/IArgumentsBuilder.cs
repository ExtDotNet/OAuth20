// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Builders.Generic;

public interface IArgumentsBuilder<TResult> : IArgumentsBuilder
{
    public ValueTask<TResult> BuildArgumentsAsync(HttpContext httpContext);
}
