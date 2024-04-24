// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.DataSources;

public interface IDataSourceContext
{
    public Type ClientDataSourceType { get; set; }

    public Type ClientSecretDataSourceType { get; set; }

    public Type EndUserDataSourceType { get; set; }

    public Type FlowDataSourceType { get; set; }

    public Type ResourceDataSourceType { get; set; }

    public Type ScopeDataSourceType { get; set; }

    public Type TokenTypeDataSourceType { get; set; }
}
