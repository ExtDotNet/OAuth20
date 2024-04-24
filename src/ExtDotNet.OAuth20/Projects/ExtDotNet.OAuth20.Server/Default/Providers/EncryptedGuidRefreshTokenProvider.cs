// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Providers;
using ExtDotNet.OAuth20.Server.Models;
using System.Text;

namespace ExtDotNet.OAuth20.Server.Default.Providers;

public class EncryptedGuidRefreshTokenProvider : IRefreshTokenProvider
{
    private static readonly string _refreshTokenSalt = Guid.NewGuid().ToString("N");
    private static readonly string _encryptionKey = Guid.NewGuid().ToString("N");

    public Task<string> GetRefreshTokenValueAsync(AccessTokenResult accessToken)
    {
        string guid = Guid.NewGuid().ToString("N");

        string originRefreshToken = $"{_refreshTokenSalt}{guid}";

        StringBuilder sb = new();

        for (int i = 0, j = 0; i < originRefreshToken.Length; i++, j++)
        {
            if (j == _encryptionKey.Length) j = 0;

            int encryptedSymbol = originRefreshToken[i] ^ _encryptionKey[j];

            sb.Append(encryptedSymbol);
        }

        string encryptedRfreshToken = sb.ToString();

        return Task.FromResult(encryptedRfreshToken);
    }
}
