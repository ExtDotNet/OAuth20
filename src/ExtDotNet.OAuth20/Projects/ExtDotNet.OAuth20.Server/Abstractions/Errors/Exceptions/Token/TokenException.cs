// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Token;

public class TokenException : OAuth20Exception
{
    public TokenException()
    {
    }

    public TokenException(string? message, string? state = null)
        : base(message, state)
    {
    }

    public TokenException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
