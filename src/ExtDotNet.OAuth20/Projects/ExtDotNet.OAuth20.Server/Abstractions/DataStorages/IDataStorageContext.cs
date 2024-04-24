// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

namespace ExtDotNet.OAuth20.Server.Abstractions.DataStorages;

public interface IDataStorageContext
{
    public Type AccessTokenStorageType { get; set; }

    public Type AuthorizationCodeStorageType { get; set; }

    public Type RefreshTokenStorageType { get; set; }

    public Type EndUserClientScopeStorageType { get; set; }
}
