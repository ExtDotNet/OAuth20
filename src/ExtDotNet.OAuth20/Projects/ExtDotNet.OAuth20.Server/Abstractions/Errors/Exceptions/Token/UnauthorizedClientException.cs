// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Token;

public class UnauthorizedClientException : TokenException
{
    public UnauthorizedClientException()
    {
    }

    public UnauthorizedClientException(string? message, string? state = null)
        : base(message, state)
    {
    }

    public UnauthorizedClientException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
