// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Attributes;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace ExtDotNet.OAuth20.Server.Options.ServerSigningCredentials.Enumerations;

public enum SecurityAlgorithmType
{
    [FieldName("undefined")]
    Undefined = 0,

    [FieldName(SecurityAlgorithms.RsaSha256)]
    RsaSha256 = 1,

    [FieldName(SecurityAlgorithms.RsaSha384)]
    RsaSha384 = 2,

    [FieldName(SecurityAlgorithms.RsaSha512)]
    RsaSha512 = 3,

    [FieldName(SecurityAlgorithms.RsaSsaPssSha256)]
    RsaSsaPssSha256 = 4,

    [FieldName(SecurityAlgorithms.RsaSsaPssSha384)]
    RsaSsaPssSha384 = 5,

    [FieldName(SecurityAlgorithms.RsaSsaPssSha512)]
    RsaSsaPssSha512 = 6,

    [FieldName(SecurityAlgorithms.EcdsaSha256)]
    EcdsaSha256 = 7,

    [FieldName(SecurityAlgorithms.EcdsaSha384)]
    EcdsaSha384 = 8,

    [FieldName(SecurityAlgorithms.EcdsaSha512)]
    EcdsaSha512 = 9,
}

public static class SecurityAlgorithmTypeExtensions
{
    public static string GetFieldNameAttributeValue(this SecurityAlgorithmType securityAlgorithmTypeName, OAuth20ServerOptions? options)
    {
        string clientSecretTypeName = securityAlgorithmTypeName switch
        {
            SecurityAlgorithmType.Undefined => securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            SecurityAlgorithmType.RsaSha256 => options?.ServerSigningCredentials?.SelfSigningCredentialsList?.RsaSha256Name ?? securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            SecurityAlgorithmType.RsaSha384 => options?.ServerSigningCredentials?.SelfSigningCredentialsList?.RsaSha384Name ?? securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            SecurityAlgorithmType.RsaSha512 => options?.ServerSigningCredentials?.SelfSigningCredentialsList?.RsaSha512Name ?? securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            SecurityAlgorithmType.RsaSsaPssSha256 => options?.ServerSigningCredentials?.SelfSigningCredentialsList?.RsaSsaPssSha256Name ?? securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            SecurityAlgorithmType.RsaSsaPssSha384 => options?.ServerSigningCredentials?.SelfSigningCredentialsList?.RsaSsaPssSha384Name ?? securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            SecurityAlgorithmType.RsaSsaPssSha512 => options?.ServerSigningCredentials?.SelfSigningCredentialsList?.RsaSsaPssSha512Name ?? securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            SecurityAlgorithmType.EcdsaSha256 => options?.ServerSigningCredentials?.SelfSigningCredentialsList?.EcdsaSha256Name ?? securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            SecurityAlgorithmType.EcdsaSha384 => options?.ServerSigningCredentials?.SelfSigningCredentialsList?.EcdsaSha384Name ?? securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            SecurityAlgorithmType.EcdsaSha512 => options?.ServerSigningCredentials?.SelfSigningCredentialsList?.EcdsaSha512Name ?? securityAlgorithmTypeName.GetFieldNameAttributeValue(),
            _ => throw new NotSupportedException($"{nameof(securityAlgorithmTypeName)}:{securityAlgorithmTypeName}"),
        };

        return clientSecretTypeName;
    }

    public static string GetFieldNameAttributeValue(this SecurityAlgorithmType securityAlgorithmTypeName)
    {
        var member = typeof(SecurityAlgorithmType).GetMember(securityAlgorithmTypeName.ToString()).First();
        string description = member.GetCustomAttribute<FieldNameAttribute>()!.Name;

        return description;
    }
}
