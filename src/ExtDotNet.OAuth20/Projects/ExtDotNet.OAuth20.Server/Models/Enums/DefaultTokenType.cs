// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Attributes;
using ExtDotNet.OAuth20.Server.Options;
using System.ComponentModel;
using System.Reflection;

namespace ExtDotNet.OAuth20.Server.Models.Enums;

/// <summary>
/// Enum with the token types are predefined by the server.
/// </summary>
public enum DefaultTokenType
{
    /// <summary>
    /// Token type by default. Invalid value for processing.
    /// </summary>
    [Description("Token type by default. Invalid value for processing.")]
    [FieldName("Undefined")]
    Undefined = 0,

    /// <summary>
    /// Description RFC7519: <see cref="https://datatracker.ietf.org/doc/html/rfc7617"/>
    /// </summary>
    [FieldName("Basic")]
    Basic = 1,

    /// <summary>
    /// Description RFC7519: <see cref="https://datatracker.ietf.org/doc/html/rfc7519"/>
    /// </summary>
    [FieldName("JWT")]
    Jwt = 2,

    /// <summary>
    /// Description RFC7519: <see cref="https://datatracker.ietf.org/doc/html/draft-ietf-oauth-v2-http-mac-05"/>
    /// </summary>
    [FieldName("MAC")]
    Mac = 3,
}

public static class DefaultTokenTypeExtensions
{
    public static string GetFieldNameAttributeValue(this DefaultTokenType defaultTokenType, OAuth20ServerOptions? options)
    {
        string tokenType = defaultTokenType switch
        {
            DefaultTokenType.Undefined => GetFieldNameAttributeValue(defaultTokenType),
            DefaultTokenType.Basic => options?.Tokens?.BasicTokenTypeName ?? GetFieldNameAttributeValue(defaultTokenType),
            DefaultTokenType.Jwt => options?.Tokens?.JwtTokenTypeName ?? GetFieldNameAttributeValue(defaultTokenType),
            DefaultTokenType.Mac => options?.Tokens?.MacTokenTypeName ?? GetFieldNameAttributeValue(defaultTokenType),
            _ => throw new NotSupportedException($"{nameof(defaultTokenType)}:{defaultTokenType}"),
        };

        return tokenType;
    }

    public static string GetFieldNameAttributeValue(this DefaultTokenType defaultTokenType)
    {
        var member = typeof(DefaultTokenType).GetMember(defaultTokenType.ToString()).First();
        string fieldName = member.GetCustomAttribute<FieldNameAttribute>()!.Name;

        return fieldName;
    }
}
