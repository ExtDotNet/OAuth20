// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Authorize;

public class TemporarilyUnavailableException : AuthorizeException
{
    public TemporarilyUnavailableException()
    {
    }

    public TemporarilyUnavailableException(string? message, string? state = null)
        : base(message, state)
    {
    }

    public TemporarilyUnavailableException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
