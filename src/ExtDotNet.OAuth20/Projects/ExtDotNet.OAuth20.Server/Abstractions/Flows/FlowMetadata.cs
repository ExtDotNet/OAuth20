// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Flows;

public class FlowMetadata
{
    protected FlowMetadata(
        string name,
        string? grantType,
        string? responseType,
        Type abstraction, string?
        description = null)
    {
        Name = name;
        GrantTypeName = grantType;
        ResponseTypeName = responseType;
        Abstraction = abstraction;
        Description = description;
    }

    public virtual string Name { get; set; }

    public virtual string? GrantTypeName { get; set; }

    public virtual string? ResponseTypeName { get; set; }

    public virtual Type Abstraction { get; set; } = default!;

    public virtual string? Description { get; set; }

    public static FlowMetadata Create<TAbstraction>(string name, string? grantType, string? responseType, string? description = null)
        where TAbstraction : IFlow
        => Create(name, grantType, responseType, typeof(TAbstraction), description);

    public static FlowMetadata Create(string name, string? grantType, string? responseType, Type abstraction, string? description = null)
    {
        if (!abstraction.IsAssignableTo(typeof(IFlow)))
        {
            throw new ArgumentException(nameof(abstraction));
        }

        if (grantType is null && responseType is null)
        {
            throw new InvalidOperationException($"At least one argument must be provided: [{nameof(grantType)}] or [{nameof(responseType)}].");
        }

        return new(name, grantType, responseType, abstraction, description);
    }
}
