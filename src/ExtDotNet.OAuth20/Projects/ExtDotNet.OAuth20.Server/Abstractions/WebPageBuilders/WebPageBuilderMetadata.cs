// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.WebPageBuilders;

public class WebPageBuilderMetadata
{
    protected WebPageBuilderMetadata(string route, Type abstraction, string? description = null)
    {
        Route = route;
        Abstraction = abstraction;
        Description = description;
    }

    public virtual string Route { get; set; } = default!;

    public virtual Type Abstraction { get; set; } = default!;

    public virtual string? Description { get; set; }

    public static WebPageBuilderMetadata Create<TAbstraction>(string route, string? description = null)
        where TAbstraction : IWebPageBuilder
        => Create(route, typeof(TAbstraction), description);

    public static WebPageBuilderMetadata Create(string route, Type abstraction, string? description = null)
    {
        if (!abstraction.IsAssignableTo(typeof(IWebPageBuilder)))
        {
            throw new ArgumentException(nameof(abstraction));
        }

        return new(route, abstraction, description);
    }
}
