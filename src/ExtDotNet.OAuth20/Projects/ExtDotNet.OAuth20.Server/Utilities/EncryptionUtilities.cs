// Developed and maintained by ExtDotNet.
// ExtDotNet licenses this file to you under the MIT license.

using System.Security.Cryptography;
using System.Text;

namespace ExtDotNet.OAuth20.Server.Utilities;

public static class EncryptionUtilities
{
    public static byte[] GetRandomBytes()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[16];
        rng.GetBytes(bytes);

        return bytes;
    }

    public static byte[] GetEncryptionBytes(string? initialValue = null)
    {
        if (initialValue is not null) return Encoding.UTF8.GetBytes(initialValue);
        else return GetRandomBytes();
    }

    public static string EncryptString(string value, string? encryptionKey = null)
    {
        // Create and initialize AES symmetric encryption algorithm:
        using Aes aes = Aes.Create();
        aes.Key = GetEncryptionBytes(encryptionKey);
        aes.GenerateIV();

        // Create encryptor to encrypt passing data:
        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        // Create streams to store, encrypt and write data:
        using MemoryStream ms = new();
        using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
        using StreamWriter sw = new(cs);

        // Write data to be encrypted:
        sw.Write(value);

        // Get a random AES value, encrypted string and create a result array:
        byte[] iv = aes.IV;
        byte[] encryptedValue = ms.ToArray();
        byte[] result = new byte[iv.Length + encryptedValue.Length];

        // Write AES value, encrypted string into the result array:
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encryptedValue, 0, result, iv.Length, encryptedValue.Length);

        // Convert result array into base64 value:
        string encryptedBase64Value = Convert.ToBase64String(result);

        return encryptedBase64Value;
    }
}
