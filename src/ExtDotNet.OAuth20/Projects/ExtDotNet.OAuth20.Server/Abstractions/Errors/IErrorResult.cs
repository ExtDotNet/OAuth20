// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.ComponentModel;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors;

public interface IErrorResult : IResult
{
    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1
    /// </summary>
    [Description("error")]
    public string Error { get; set; }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1
    /// </summary>
    [Description("error_description")]
    public string? ErrorDescription { get; set; }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1
    /// </summary>
    [Description("error_uri")]
    public string? ErrorUri { get; set; }

    /// <summary>
    /// Description RFC6749: https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.2.1
    /// </summary>
    [Description("state")]
    public string? State { get; set; }
}
