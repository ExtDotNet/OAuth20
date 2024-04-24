// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Token;

public class InvalidRequestException : TokenException
{
    public InvalidRequestException()
    {
    }

    public InvalidRequestException(string? message, string? state = null)
        : base(message, state)
    {
    }

    public InvalidRequestException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
