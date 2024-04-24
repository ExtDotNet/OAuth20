// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Token;

public class InvalidScopeException : TokenException
{
    public InvalidScopeException()
    {
    }

    public InvalidScopeException(string? message, string? state = null)
        : base(message, state)
    {
    }

    public InvalidScopeException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
