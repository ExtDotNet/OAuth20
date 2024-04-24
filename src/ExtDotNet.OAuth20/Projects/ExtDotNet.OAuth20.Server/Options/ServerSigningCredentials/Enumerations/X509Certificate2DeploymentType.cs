// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Attributes;
using System.Reflection;

namespace ExtDotNet.OAuth20.Server.Options.ServerSigningCredentials.Enumerations;

public enum X509Certificate2DeploymentType
{
    [FieldName("undefined")]
    Undefined = 0,

    [FieldName("pem")]
    Pem = 1,

    [FieldName("encrypted_pem")]
    EncryptedPem = 2,

    [FieldName("pem_file")]
    PemFile = 3,

    [FieldName("encrypted_pem_file")]
    EncryptedPemFile = 4,
}

public static class X509Certificate2DeploymentTypeExtensions
{
    public static string GetFieldNameAttributeValue(this X509Certificate2DeploymentType x509Certificate2DeploymentTypeName, OAuth20ServerOptions? options)
    {
        string clientSecretTypeName = x509Certificate2DeploymentTypeName switch
        {
            X509Certificate2DeploymentType.Undefined => x509Certificate2DeploymentTypeName.GetFieldNameAttributeValue(),
            X509Certificate2DeploymentType.Pem => options?.ServerSigningCredentials?.DeploymentTypePemName ?? x509Certificate2DeploymentTypeName.GetFieldNameAttributeValue(),
            X509Certificate2DeploymentType.EncryptedPem => options?.ServerSigningCredentials?.DeploymentTypeEncryptedPemName ?? x509Certificate2DeploymentTypeName.GetFieldNameAttributeValue(),
            X509Certificate2DeploymentType.PemFile => options?.ServerSigningCredentials?.DeploymentTypePemFileName ?? x509Certificate2DeploymentTypeName.GetFieldNameAttributeValue(),
            X509Certificate2DeploymentType.EncryptedPemFile => options?.ServerSigningCredentials?.DeploymentTypeEncryptedPemFileName ?? x509Certificate2DeploymentTypeName.GetFieldNameAttributeValue(),
            _ => throw new NotSupportedException($"{nameof(x509Certificate2DeploymentTypeName)}:{x509Certificate2DeploymentTypeName}"),
        };

        return clientSecretTypeName;
    }

    public static string GetFieldNameAttributeValue(this X509Certificate2DeploymentType x509Certificate2DeploymentTypeName)
    {
        var member = typeof(X509Certificate2DeploymentType).GetMember(x509Certificate2DeploymentTypeName.ToString()).First();
        string description = member.GetCustomAttribute<FieldNameAttribute>()!.Name;

        return description;
    }
}
