// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Models.Flows;

namespace ExtDotNet.OAuth20.Server.Abstractions.Services;

public interface ILoginService
{
    public Task<IResult> RedirectToLoginAsync(FlowArguments args);
}
