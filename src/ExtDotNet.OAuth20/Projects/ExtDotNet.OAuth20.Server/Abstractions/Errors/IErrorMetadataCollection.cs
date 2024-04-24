// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.Errors;

public interface IErrorMetadataCollection
{
    public IDictionary<string, ErrorMetadata> AuthorizeErrors { get; set; }

    public IDictionary<string, ErrorMetadata> TokenErrors { get; set; }

    public IDictionary<string, ErrorMetadata> CommonErrors { get; set; }
}
