// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Runtime.Serialization;

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors.Exceptions.Common;

public class CommonErrorException : CommonException
{
    public CommonErrorException()
    {
    }

    public CommonErrorException(string? message, string? state = null)
        : base(message, state)
    {
    }

    public CommonErrorException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
