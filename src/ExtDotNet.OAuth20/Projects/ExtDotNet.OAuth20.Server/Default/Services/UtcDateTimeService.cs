// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Services;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class UtcDateTimeService : IDateTimeService
{
    public DateTime GetCurrentDateTime() => DateTime.UtcNow;

    public string ConvertDateTimeToString(DateTime dateTime) => dateTime.ToString();
}
