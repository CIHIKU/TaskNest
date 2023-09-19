using AuthService.Helpers;

namespace AuthService.Tests;

public class CryptoHelperTest
{
    [Theory]
    [InlineData("TestPlainText1")]
    [InlineData("TestPlainText2")]
    public void Decrypt_ShouldReturnDecryptedString(string plainText)
    {
        var cryptoHelper = new CryptoHelper();
        
        // Arrange
        var encryptedText = cryptoHelper.Encrypt(plainText);

        // Act
        var decryptedText = cryptoHelper.Decrypt(encryptedText);

        // Assert
        Assert.NotNull(decryptedText);
        Assert.Equal(plainText, decryptedText);
        Assert.NotEqual(plainText, encryptedText);
    }
}