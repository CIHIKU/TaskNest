namespace AuthService.Helpers;

public interface ICryptoHelper
{
    public string Encrypt(string plainText);
    public string Decrypt(string cipherText);
}