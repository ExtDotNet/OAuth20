// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Domain.Abstractions;

namespace ExtDotNet.OAuth20.Server.Domain;

public class TokenTypeTokenAdditionalParameter : Int32IdEntityBase
{
    public int TokenTypeId { get; set; }

    public TokenType TokenType { get; set; } = default!;

    public int TokenAdditionalParameterId { get; set; }

    public TokenAdditionalParameter TokenAdditionalParameter { get; set; } = default!;
}
