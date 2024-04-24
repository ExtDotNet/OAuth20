// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Authorize;

public class UnsupportedResponseTypeException : AuthorizeException
{
    public UnsupportedResponseTypeException()
    {
    }

    public UnsupportedResponseTypeException(string? message, string? state = null)
        : base(message, state)
    {
    }

    public UnsupportedResponseTypeException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
