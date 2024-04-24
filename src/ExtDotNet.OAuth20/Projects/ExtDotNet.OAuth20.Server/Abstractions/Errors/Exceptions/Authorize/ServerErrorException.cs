// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Authorize;

public class ServerErrorException : AuthorizeException
{
    public ServerErrorException()
    {
    }

    public ServerErrorException(string? message, string? state = null)
        : base(message, state)
    {
    }

    public ServerErrorException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
