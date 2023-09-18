using AuthService.Helpers;

namespace AuthService.Tests;

public class CryptoHelperTest
{
    [Theory]
    [InlineData("TestPlainText1")]
    [InlineData("TestPlainText2")]
    public void Encrypt_ShouldReturnEncryptedString(string plainText)
    {
        // Act
        var encryptedText = CryptoHelper.Encrypt(plainText);

        // Assert
        Assert.NotNull(encryptedText);
        Assert.NotEqual(plainText, encryptedText);
    }
    
    [Theory]
    [InlineData("TestPlainText1")]
    [InlineData("TestPlainText2")]
    public void Decrypt_ShouldReturnDecryptedString(string plainText)
    {
        // Arrange
        var encryptedText = CryptoHelper.Encrypt(plainText);

        // Act
        var decryptedText = CryptoHelper.Decrypt(encryptedText);

        // Assert
        Assert.NotNull(decryptedText);
        Assert.Equal(plainText, decryptedText);
    }
}