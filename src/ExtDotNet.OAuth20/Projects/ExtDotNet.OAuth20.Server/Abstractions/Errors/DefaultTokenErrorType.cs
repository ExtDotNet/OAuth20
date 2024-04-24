// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Attributes;
using ExtDotNet.OAuth20.Server.Options;
using System.Reflection;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors;

/// <summary>
/// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-5.2
/// </summary>
public enum DefaultTokenErrorType
{
    /// <summary>
    /// The error is not defined. Invalid value for a response.
    /// </summary>
    [FieldName("undefined")]
    Undefined = 0,

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-5.2
    /// </summary>
    [FieldName("invalid_request")]
    InvalidRequest = 1,

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-5.2
    /// </summary>
    [FieldName("invalid_client")]
    InvalidClient = 2,

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-5.2
    /// </summary>
    [FieldName("invalid_grant")]
    InvalidGrant = 3,

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-5.2
    /// </summary>
    [FieldName("unauthorized_client")]
    UnauthorizedClient = 4,

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-5.2
    /// </summary>
    [FieldName("unsupported_grant_type")]
    UnsupportedGrantType = 5,

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-5.2
    /// </summary>
    [FieldName("invalid_scope")]
    InvalidScope = 6,
}

public static class DefaultTokenErrorTypeExtensions
{
    public static string GetFieldNameAttributeValue(this DefaultTokenErrorType defaultErrorType, OAuth20ServerOptions? options)
    {
        string errorCode = defaultErrorType switch
        {
            DefaultTokenErrorType.Undefined => GetFieldNameAttributeValue(defaultErrorType),
            DefaultTokenErrorType.InvalidRequest => options?.Errors?.TokenInvalidRequestErrorCode ?? GetFieldNameAttributeValue(defaultErrorType),
            DefaultTokenErrorType.InvalidClient => options?.Errors?.TokenInvalidClientErrorCode ?? GetFieldNameAttributeValue(defaultErrorType),
            DefaultTokenErrorType.InvalidGrant => options?.Errors?.TokenInvalidGrantErrorCode ?? GetFieldNameAttributeValue(defaultErrorType),
            DefaultTokenErrorType.UnauthorizedClient => options?.Errors?.TokenUnauthorizedClientErrorCode ?? GetFieldNameAttributeValue(defaultErrorType),
            DefaultTokenErrorType.UnsupportedGrantType => options?.Errors?.TokenUnsupportedGrantTypeErrorCode ?? GetFieldNameAttributeValue(defaultErrorType),
            DefaultTokenErrorType.InvalidScope => options?.Errors?.TokenInvalidScopeErrorCode ?? GetFieldNameAttributeValue(defaultErrorType),
            _ => throw new NotSupportedException($"{nameof(defaultErrorType)}:{defaultErrorType}"),
        };

        return errorCode;
    }

    public static string GetFieldNameAttributeValue(this DefaultTokenErrorType defaultErrorType)
    {
        var member = typeof(DefaultTokenErrorType).GetMember(defaultErrorType.ToString()).First();
        string description = member.GetCustomAttribute<FieldNameAttribute>()!.Name;

        return description;
    }
}
