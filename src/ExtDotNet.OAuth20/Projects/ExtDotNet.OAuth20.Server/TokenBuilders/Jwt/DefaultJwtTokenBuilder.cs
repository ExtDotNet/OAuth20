// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;
using ExtDotNet.OAuth20.Server.Abstractions.ServerSigningCredentials;
using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Domain;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace ExtDotNet.OAuth20.Server.TokenBuilders.Jwt;

/// <summary>
/// Description RFC7519: <see cref="https://datatracker.ietf.org/doc/html/rfc7519"/>
/// </summary>
public class DefaultJwtTokenBuilder : IJwtTokenBuilder
{
    private readonly IOptions<OAuth20ServerOptions> _options;
    private readonly IServerMetadataService _serverMetadataService;
    private readonly ISigningCredentialsAlgorithmsService _signingCredentialsAlgorithmsService;
    private readonly IServerSigningCredentialsProvider _serverSigningCredentialsProvider;

    public DefaultJwtTokenBuilder(
        IOptions<OAuth20ServerOptions> options,
        IServerMetadataService serverMetadataService,
        ISigningCredentialsAlgorithmsService signingCredentialsAlgorithmsService,
        IServerSigningCredentialsProvider serverSigningCredentialsProvider)
    {
        _options = options;
        _serverMetadataService = serverMetadataService;
        _signingCredentialsAlgorithmsService = signingCredentialsAlgorithmsService;
        _serverSigningCredentialsProvider = serverSigningCredentialsProvider;
    }

    public async ValueTask<string> BuildTokenAsync(Models.TokenContext tokenBuilderContext)
    {
        var jwtHeader = await BuildJwtTokenHeaderAsync(tokenBuilderContext.Scopes).ConfigureAwait(false);
        var jwtPayload = await BuildJwtTokenPayloadAsync(tokenBuilderContext).ConfigureAwait(false);

        var jwtSecurityToken = await BuildJwtSecurityTokenAsync(jwtHeader, jwtPayload).ConfigureAwait(false);
        var jwtSecurityTokenString = await BuildJwtSecurityTokenStringAsync(jwtSecurityToken).ConfigureAwait(false);

        return jwtSecurityTokenString;
    }

    private static ValueTask<string> BuildJwtSecurityTokenStringAsync(JwtSecurityToken jwtSecurityToken)
    {
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        string jwtSecurityTokenString = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        return ValueTask.FromResult(jwtSecurityTokenString);
    }

    private static ValueTask<JwtPayload> BuildJwtTokenPayloadAsync(Models.TokenContext tokenBuilderContext)
    {
        JwtPayload jwtPayload = new(
            issuer: tokenBuilderContext.Issuer,
            audience: null,
            claims: null,
            notBefore: tokenBuilderContext.ActivationDateTime?.UtcDateTime.ToUniversalTime(),
            expires: tokenBuilderContext.ExpirationDateTime?.UtcDateTime.ToUniversalTime(),
            issuedAt: tokenBuilderContext.CreationDateTime?.UtcDateTime.ToUniversalTime());

        if (tokenBuilderContext.Audiences?.Any() is not true)
        {
            if (tokenBuilderContext.Scopes?.Any() is not true)
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

        foreach (var audience in tokenBuilderContext.Audiences)
        {
            jwtPayload.AddClaim(new Claim("aud", audience));
        }

        if (tokenBuilderContext.Scopes?.Any() is not true)
        {
            throw new InvalidRequestException(
                "At least one [Scope] must be specified to create a token, " +
                "but no [Scope] is specified in the current request.");
        }

        foreach (Scope scope in tokenBuilderContext.Scopes)
        {
            jwtPayload.AddClaim(new Claim("scope", scope.Name));
        }

        return ValueTask.FromResult(jwtPayload);
    }

    private static ValueTask<JwtSecurityToken> BuildJwtSecurityTokenAsync(JwtHeader jwtHeader, JwtPayload jwtPayload) =>
         ValueTask.FromResult(new JwtSecurityToken(jwtHeader, jwtPayload));

    private async Task<JwtHeader> BuildJwtTokenHeaderAsync(IEnumerable<Scope> scopes)
    {
        var signingCredentialsAlgorithms = await _signingCredentialsAlgorithmsService
            .GetSigningCredentialsAlgorithmsForScopesAsync(scopes)
            .ConfigureAwait(false);

        var signingCredentialsList = await _serverSigningCredentialsProvider
            .GetSigningCredentialsAsync(signingCredentialsAlgorithms)
            .ConfigureAwait(false);

        SigningCredentials signingCredentials;

        if (signingCredentialsList.Any())
        {
            signingCredentials = signingCredentialsList.First();
        }
        else
        {
            signingCredentials = await _serverSigningCredentialsProvider.GetDefaultSigningCredentialsAsync().ConfigureAwait(false);
        }

        JwtHeader jwtHeader = new(signingCredentials);

        if (signingCredentials.Key is X509SecurityKey x509SecurityKey)
        {
            X509Certificate2 x509Certificate2 = x509SecurityKey.Certificate;

            jwtHeader["x5t"] = WebEncoders.Base64UrlEncode(x509Certificate2.GetCertHash());
        }

        jwtHeader["typ"] = "JWT";

        return jwtHeader;
    }
}
