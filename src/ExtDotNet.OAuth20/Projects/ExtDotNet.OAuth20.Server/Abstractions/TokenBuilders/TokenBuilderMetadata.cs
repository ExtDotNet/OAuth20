// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.TokenBuilders;

public class TokenBuilderMetadata
{
    protected TokenBuilderMetadata(string tokenType, Type abstraction, string? description = null, IDictionary<string, string>? additionalParameters = null)
    {
        TokenType = tokenType;
        Abstraction = abstraction;
        Description = description;
        AdditionalParameters = additionalParameters;
    }

    public virtual string TokenType { get; set; } = default!;

    public virtual Type Abstraction { get; set; } = default!;

    public virtual string? Description { get; set; }

    public IDictionary<string, string>? AdditionalParameters { get; set; }

    public static TokenBuilderMetadata Create<TAbstraction>(string type, string? description = null, IDictionary<string, string>? additionalParameters = null)
        where TAbstraction : ITokenBuilder
        => Create(type, typeof(TAbstraction), description, additionalParameters);

    public static TokenBuilderMetadata Create(string type, Type abstraction, string? description = null, IDictionary<string, string>? additionalParameters = null)
    {
        if (!abstraction.IsAssignableTo(typeof(ITokenBuilder)))
        {
            throw new ArgumentException(nameof(abstraction));
        }

        return new(type, abstraction, description, additionalParameters);
    }
}
