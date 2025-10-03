using System.Text;
using System.Security.Cryptography;

namespace Haviliar.Infra.Security;

public static class IdCryptService
{
    private static readonly byte[] Iv =
    {
        12,
        34,
        56,
        78,
        90,
        102,
        114,
        126,
        149,
        155,
        189,
        192,
        210,
        219,
        225,
        252,
    };
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("A1b2C3d4E5f6G7h8A1b2C3d4E5f6G7h8");

    public static string EncryptId(int id)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;

        var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(id.ToString());

        var encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert
            .ToBase64String(encrypted)
            .Replace("+", "_a0_")
            .Replace("\\", "_b1_")
            .Replace("/", "_c2_")
            .Replace(":", "_d3_")
            .Replace("*", "_e4_")
            .Replace("?", "_f5_")
            .Replace("\"", "_g6_")
            .Replace("<", "_h7_")
            .Replace(">", "_i8_")
            .Replace("|", "_j9_");
    }

    public static int DecryptId(string encryptedId)
    {
        encryptedId = encryptedId
            .Replace("_a0_", "+")
            .Replace("_b1_", "\\")
            .Replace("_c2_", "/")
            .Replace("_d3_", ":")
            .Replace("_e4_", "*")
            .Replace("_f5_", "?")
            .Replace("_g6_", "\"")
            .Replace("_h7_", "<")
            .Replace("_i8_", ">")
            .Replace("_j9_", "|");

        var cipherBytes = Convert.FromBase64String(encryptedId);

        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = Iv;

        var decryptor = aes.CreateDecryptor();
        var decrypted = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

        return int.Parse(Encoding.UTF8.GetString(decrypted));
    }
}
