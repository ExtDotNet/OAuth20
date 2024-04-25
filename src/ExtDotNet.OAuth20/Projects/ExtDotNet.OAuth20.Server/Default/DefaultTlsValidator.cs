// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions;
using System.Security.Cryptography.X509Certificates;

namespace ExtDotNet.OAuth20.Server.Default;

public class DefaultTlsValidator : ITlsValidator
{
    public OAuth20ValidationResult TryValidate(HttpContext httpContext)
    {
        if (!httpContext.Request.IsHttps)
        {
            return new OAuth20ValidationResult
            {
                Success = false,
                Description = "The request must use HTTPS with SSL/TLS certificate."
            };
        }

        var certificate = httpContext.Connection.ClientCertificate;

        if (certificate == null)
        {
            return new OAuth20ValidationResult
            {
                Success = false,
                Description = "No TLS certificate provided."
            };
        }

        if (!TryValidateCertificate(certificate))
        {
            return new OAuth20ValidationResult
            {
                Success = false,
                Description = "The TLS certificate is invalid."
            };
        }

        return new OAuth20ValidationResult { Success = true };
    }

    private static bool TryValidateCertificate(X509Certificate2 certificate)
    {
        using X509Chain chain = new();

        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
        chain.ChainPolicy.VerificationTime = DateTime.Now;
        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);

        return chain.Build(certificate);
    }
}
