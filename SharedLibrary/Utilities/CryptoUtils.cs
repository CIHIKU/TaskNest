using System.Security.Cryptography;

namespace SharedLibrary.Utilities;

public static class CryptoUtils
{
    public static byte[] GenerateRandomBytes(int byteSize = 32)
    {
        var randomNumber = new byte[byteSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return randomNumber;
    }
}