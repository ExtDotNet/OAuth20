// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Attributes;
using ExtDotNet.OAuth20.Server.Options;
using System.ComponentModel;
using System.Reflection;

namespace ExtDotNet.OAuth20.Server.Models.Enums;

public enum DefaultTokenOwnerType
{
    /// <summary>
    /// Token owner type by default. Invalid value for processing.
    /// </summary>
    [Description("Token type by default. Invalid value for processing.")]
    [FieldName("Undefined")]
    Undefined = 0,

    [FieldName("Client")]
    Client = 1,

    [FieldName("EndUser")]
    EndUser = 2,
}

public static class DefaultTokenOwnerTypeExtensions
{
    public static string GetFieldNameAttributeValue(this DefaultTokenOwnerType defaultTokenOwnerType, OAuth20ServerOptions? options)
    {
        string tokenOwnerType = defaultTokenOwnerType switch
        {
            DefaultTokenOwnerType.Undefined => GetFieldNameAttributeValue(defaultTokenOwnerType),
            DefaultTokenOwnerType.Client => options?.Tokens?.ClientTokenOwnerTypeName ?? GetFieldNameAttributeValue(defaultTokenOwnerType),
            DefaultTokenOwnerType.EndUser => options?.Tokens?.EndUserTokenOwnerTypeName ?? GetFieldNameAttributeValue(defaultTokenOwnerType),
            _ => throw new NotSupportedException($"{nameof(defaultTokenOwnerType)}:{defaultTokenOwnerType}"),
        };

        return tokenOwnerType;
    }

    public static string GetFieldNameAttributeValue(this DefaultTokenOwnerType defaultTokenOwnerType)
    {
        var member = typeof(DefaultTokenOwnerType).GetMember(defaultTokenOwnerType.ToString()).First();
        string fieldName = member.GetCustomAttribute<FieldNameAttribute>()!.Name;

        return fieldName;
    }
}
