// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;

public class ServerConfigurationErrorException : CommonException
{
    public ServerConfigurationErrorException()
    {
    }

    public ServerConfigurationErrorException(string? message, string? state = null)
        : base(message, state)
    {
    }

    public ServerConfigurationErrorException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
