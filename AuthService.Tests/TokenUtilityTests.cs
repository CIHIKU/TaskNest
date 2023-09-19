using AuthService.Helpers;
using AuthService.Utilities;
using Microsoft.IdentityModel.Tokens;
using Moq;
using TokenValidationResult = AuthService.Utilities.TokenValidationResult;

namespace AuthService.Tests;

public class TokenUtilityTests
{
    private static readonly Mock<ICryptoHelper> CryptoHelperMock = new();
    private TokenUtility? _tokenUtility;

    private readonly TokenValidationParameters _tokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = null,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    static TokenUtilityTests()
    {
        CryptoHelperMock.Setup(x => 
                x.Encrypt(It.IsAny<string>()))
            .Returns((string s) => "encrypted_" + s);
    }
    
    public static IEnumerable<object[]> GetTestEncryptedClaimsData()
    {
        yield return new object[] { GetTestEncryptedClaims("Lol") };
        yield return new object[] { GetTestEncryptedClaims("Mac&Cheese") };
    }
    
    private static string GetTestEncryptedClaims(string claims)
    {
        return CryptoHelperMock.Object.Encrypt(claims);
    }

    [Theory]
    [MemberData(nameof(GetTestEncryptedClaimsData))]
    public void ValidateToken_WithValidToken_ShouldReturnValidResult(string encryptedClaims)
    {
        _tokenUtility = new TokenUtility(_tokenValidationParameters, 1);
        
        var token = _tokenUtility.GenerateToken(encryptedClaims);
        
        var (result, principal) = _tokenUtility.ValidateToken(token);
        
        Assert.Equal(TokenValidationResult.Valid, result);
        Assert.NotNull(principal);
        
        var claimValue = principal.Claims.FirstOrDefault(c => c.Type == "TokenClaims")?.Value;
        Assert.Equal(encryptedClaims, claimValue);
        
    }
    [Theory]
    [MemberData(nameof(GetTestEncryptedClaimsData))]
    public async Task ValidateToken_WithExpiredToken_ShouldReturnExpiredResult(string encryptedClaims)
    {
        // Adjust the expiration time to a very short period for this test
        _tokenUtility = new TokenUtility(_tokenValidationParameters, -1);

        var token = _tokenUtility.GenerateToken(encryptedClaims);

        // Wait for the token to expire
        await Task.Delay(2000);

        var (result, principal) = _tokenUtility.ValidateToken(token);
    
        Assert.Equal(TokenValidationResult.Expired, result);
        Assert.Null(principal);
    }
    
    [Theory]
    [MemberData(nameof(GetTestEncryptedClaimsData))]
    public void ValidateToken_WithInvalidToken_ShouldReturnInvalidResult(string encryptedClaims)
    {
        _tokenUtility = new TokenUtility(_tokenValidationParameters, 1);

        // Generate a valid token first
        var token = _tokenUtility.GenerateToken(encryptedClaims);

        // Now tamper with the token to make it invalid
        var invalidToken = token + "tamper";

        // Validate the tampered token
        var (result, principal) = _tokenUtility.ValidateToken(invalidToken);

        Assert.Equal(TokenValidationResult.Invalid, result);
        Assert.Null(principal);
    }
}