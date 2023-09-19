using System.Security.Cryptography;
using System.Text;

namespace AuthService.Helpers;

public class CryptoHelper : ICryptoHelper
{
    private readonly byte[] _aesKey;

    private readonly byte[] _aesIv;

    public CryptoHelper()
    {
        _aesKey = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AES_KEY") ?? "MySecretKey_1234");
        _aesIv = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AES_IV") ?? "MySecretIV__1234");
        
        if (_aesKey.Length is not (16 or 24 or 32))
        {
            throw new Exception("Invalid AES key size. It must be 16, 24, or 32 bytes.");
        }

        if (_aesIv.Length != 16)
        {
            throw new Exception("Invalid AES IV size. It must be 16 bytes.");
        }
    }
    
    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _aesKey;
        aes.IV = _aesIv;

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var sw = new StreamWriter(cs);
        
        sw.Write(plainText);
        sw.Flush();
        cs.FlushFinalBlock();
        
        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = _aesKey;
        aes.IV = _aesIv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }
}