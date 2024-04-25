// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Builders.Generic;
using ExtDotNet.OAuth20.Server.Models.Flows;

namespace ExtDotNet.OAuth20.Server.Default.Builders;

public class FlowArgumentsBuilder : IArgumentsBuilder<FlowArguments>
{
    public ValueTask<FlowArguments> BuildArgumentsAsync(HttpContext httpContext)
    {
        HttpRequest httpRequest = httpContext.Request;

        Dictionary<string, string> values;

        if (httpRequest.Method == HttpMethods.Post)
        {
            values = httpRequest.Form.ToDictionary(x => x.Key, x => x.Value.First()!);
        }
        else if (httpRequest.Method == HttpMethods.Get)
        {
            values = httpRequest.Query.ToDictionary(x => x.Key, x => x.Value.First()!);
        }
        else
        {
            throw new NotSupportedException($"{nameof(httpRequest.Method)}:{httpRequest.Method}");
        }

        FlowArguments flowArguments = new()
        {
            Values = values,
        };

        return ValueTask.FromResult(flowArguments);
    }
}
