// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using ExtDotNet.OAuth20.Server.Abstractions.Services;
using ExtDotNet.OAuth20.Server.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace ExtDotNet.OAuth20.Server.Default.Services;

public class DefaultPasswordHashingService(IOptions<OAuth20ServerOptions> options) : IPasswordHashingService
{
    private readonly IOptions<OAuth20ServerOptions> _options = options ?? throw new ArgumentNullException(nameof(options));

    public ValueTask<string?> GetPasswordHashAsync(string? password)
    {
        if (string.IsNullOrWhiteSpace(password)) return ValueTask.FromResult<string?>(null);

        string originSalt = _options.Value.PasswordHashingSalt ?? nameof(DefaultPasswordHashingService);

        string encryptedSalt = Convert.ToBase64String(Encoding.UTF8.GetBytes(originSalt));

        using SHA256 sha256 = SHA256.Create();

        // Combine password and salt, then convert to byte array
        byte[] combinedBytes = Encoding.UTF8.GetBytes(password + encryptedSalt);

        // Compute the hash
        byte[] hashedBytes = sha256.ComputeHash(combinedBytes);

        // Convert the hashed bytes to a hexadecimal string
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < hashedBytes.Length; i++)
        {
            builder.Append(hashedBytes[i].ToString("x2"));
        }

        string passwordHash = builder.ToString();

        return ValueTask.FromResult<string?>(passwordHash);
    }
}
