// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Options.ClientSecrets;
using ExtDotNet.OAuth20.Server.Options.Endpoints;
using ExtDotNet.OAuth20.Server.Options.Entities;
using ExtDotNet.OAuth20.Server.Options.Errors;
using ExtDotNet.OAuth20.Server.Options.Flows;
using ExtDotNet.OAuth20.Server.Options.ServerInformation;
using ExtDotNet.OAuth20.Server.Options.ServerSigningCredentials;
using ExtDotNet.OAuth20.Server.Options.Tokens;

namespace ExtDotNet.OAuth20.Server.Options;

public class OAuth20ServerOptions
{
    public const string DefaultSection = "OAuth20Server";

    public string? AuthorizationServerRelativeUriPrefix { get; set; } = "oauth20";

    public bool AuthorizationRequestStateRequired { get; set; } = true;

    public bool AuthorizationRequestScopeRequired { get; set; } = false;

    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.1.2.2"/>
    /// </summary>
    public bool RequestRedirectionUriRequired { get; set; } = false;

    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.1.2.2"/>
    /// </summary>
    public bool ClientRegistrationRedirectionEndpointsRequired { get; set; } = true;

    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.1.2.2"/>
    /// </summary>
    public bool ClientMultipleRedirectionEndpointsAllowed { get; set; } = true;

    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.1.2.2"/>
    /// </summary>
    public bool ClientRegistrationCompleteRedirectionEndpointRequired { get; set; } = true;

    /// <summary>
    /// Description RFC6749: <see cref="https://datatracker.ietf.org/doc/html/rfc6749#section-3.1.2.4"/>
    /// </summary>
    public bool InformResourceOwnerOfRedirectionUriError { get; set; } = true;

    public bool FullyIgnoreRequestedScopes { get; set; } = false;

    public IEnumerable<string>? SelectivelyIgnoredRequestedScopes { get; set; }

    public IEnumerable<string>? ScopePreDefinedDefaultValue { get; set; }

    public int? AuthorizationCodeDefaultSizeSymbols { get; set; }

    public bool InclusionScopeToResponseRequired { get; set; } = true;

    public bool UserScopeAllowanceRequired { get; set; } = true;

    public bool TokenResponseExpiresInRequired { get; set; } = true;

    public bool ClientCredentialsFlowRefreshTokenAccepted { get; set; } = false;

    public string? DefaultLoginEndpoint { get; set; }

    public string? DefaultPermissionsEndpoint { get; set; }

    public long? DefaultAuthorizationCodeExpirationSeconds { get; set; }

    public string? PasswordHashingSalt { get; set; }

    public bool EndUserPermissionsRequiredForClients { get; set; } = false;

    public bool EnableLoggingScopeInterceptor { get; set; } = false;

    public OAuth20ServerEndpointsOptions? Endpoints { get; set; }

    public OAuth20ServerFlowsOptions? Flows { get; set; }

    public OAuth20ServerErrorsOptions? Errors { get; set; }

    public OAuth20ServerEntitiesOptions? Entities { get; set; }

    public OAuth20ServerTokensOptions? Tokens { get; set; }

    public OAuth20ServerClientSecretsOptions? ClientSecrets { get; set; }

    public OAuth20ServerSigningCredentialsOptions? ServerSigningCredentials { get; set; }

    public OAuth20ServerInformationOptions? ServerInformation { get; set; }
}
