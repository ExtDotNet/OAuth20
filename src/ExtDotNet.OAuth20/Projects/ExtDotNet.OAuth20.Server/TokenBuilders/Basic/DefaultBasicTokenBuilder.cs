// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Models;
using System.Text;

namespace ExtDotNet.OAuth20.Server.TokenBuilders.Basic;

/// <summary>
/// Description RFC7519: <see cref="https://datatracker.ietf.org/doc/html/rfc7617"/>
/// </summary>
public class DefaultBasicTokenBuilder : IBasicTokenBuilder
{
    public ValueTask<string> BuildTokenAsync(TokenContext tokenBuilderContext)
    {
        StringBuilder sb = new("Type:Basic::Encoding:Base64");

        if (tokenBuilderContext.Issuer is not null)
            sb.AppendFormat("::Issuer:{0}", tokenBuilderContext.Issuer);

        if (tokenBuilderContext.Audiences is null || tokenBuilderContext.Audiences.Any())
        {
            if (tokenBuilderContext.Scopes is null || tokenBuilderContext.Scopes.Any())
            {
                throw new InvalidRequestException(
                    "At least one [Audience] must be specified to create a token, " +
                    "but no [Audience] is specified in the current request." +
                    "Most likely the Server was unable to determine the [Audience] " +
                    "who owns the requested [Scope].");
            }
            else
            {
                throw new ServerConfigurationErrorException(
                    "At least one [Audience] must be specified to create a token, " +
                    "but no [Audience] is specified in the current request." +
                    "Most likely the Server determines the [Audience] based on " +
                    "the requested [Scope], but no [Scope] is specified in the " +
                    "current request.");
            }
        }

        sb.Append("::Audience");

        foreach (var audience in tokenBuilderContext.Audiences)
        {
            sb.AppendFormat($":{0}", audience);
        }

        if (tokenBuilderContext.Scopes is null || tokenBuilderContext.Scopes.Any())
        {
            throw new InvalidRequestException(
                "At least one [Scope] must be specified to create a token, " +
                "but no [Scope] is specified in the current request.");
        }

        sb.Append("::Scope");

        foreach (Scope scope in tokenBuilderContext.Scopes)
        {
            sb.AppendFormat($":{0}", scope.Name);
        }

        if (tokenBuilderContext.ActivationDateTime is not null)
            sb.AppendFormat("::NotBefore:{0}", tokenBuilderContext.ActivationDateTime?.UtcDateTime.ToUniversalTime());

        if (tokenBuilderContext.ExpirationDateTime is not null)
            sb.AppendFormat("::Expires:{0}", tokenBuilderContext.ExpirationDateTime?.UtcDateTime.ToUniversalTime());

        if (tokenBuilderContext.CreationDateTime is not null)
            sb.AppendFormat("::IssuedAt:{0}", tokenBuilderContext.CreationDateTime?.UtcDateTime.ToUniversalTime());

        string originAccessToken = sb.ToString();
        byte[] binaryAccessToken = Encoding.ASCII.GetBytes(originAccessToken);
        string accessToken = Convert.ToBase64String(binaryAccessToken);

        return ValueTask.FromResult(accessToken);
    }
}
