// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Errors;
using System.Collections.Concurrent;

namespace ExtDotNet.OAuth20.Server.Default.Errors;

public class DefaultErrorMetadataCollection : IErrorMetadataCollection
{
    public IDictionary<string, ErrorMetadata> AuthorizeErrors { get; set; } = new ConcurrentDictionary<string, ErrorMetadata>();

    public IDictionary<string, ErrorMetadata> TokenErrors { get; set; } = new ConcurrentDictionary<string, ErrorMetadata>();

    public IDictionary<string, ErrorMetadata> CommonErrors { get; set; } = new ConcurrentDictionary<string, ErrorMetadata>();
}
