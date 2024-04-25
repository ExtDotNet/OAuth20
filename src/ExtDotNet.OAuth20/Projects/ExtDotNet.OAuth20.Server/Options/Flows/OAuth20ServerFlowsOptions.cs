// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Options.Flows;

public class OAuth20ServerFlowsOptions
{
    public const string DefaultSection = "OAuth20Server:Flows";

    public string? AuthorizationCodeFlowName { get; set; }

    public string? ImplicitFlowName { get; set; }

    public string? ClientCredentialsFlowName { get; set; }

    public string? ResourceOwnerPasswordCredentialsFlowName { get; set; }

    public string? RefreshTokenFlowName { get; set; }

    public string? AuthorizationCodeFlowResponseTypeName { get; set; }

    public string? ImplicitFlowResponseTypeName { get; set; }

    public string? AuthorizationFlowGrantTypeName { get; set; }

    public string? ClientCredentialsFlowGrantTypeName { get; set; }

    public string? ResourceOwnerPasswordCredentialsFlowGrantTypeName { get; set; }

    public string? RefreshTokenFlowGrantTypeName { get; set; }

    public string? AuthorizationCodeEncryptionKey { get; set; }

    public string? RefreshTokenEncryptionKey { get; set; }

    public IEnumerable<FlowOptions>? FlowList { get; set; }
}
