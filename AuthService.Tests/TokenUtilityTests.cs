using AuthService.Helpers;
using AuthService.Utilities;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using TokenValidationResult = AuthService.Utilities.TokenValidationResult;

namespace AuthService.Tests;

public class TokenUtilityTests
{
    private TokenUtility _tokenUtility;

    private readonly TokenValidationParameters _tokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = null,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    [Fact]
    public void ValidateToken_WithValidToken_ShouldReturnValidResult()
    {
        _tokenUtility = new TokenUtility(_tokenValidationParameters, 1);
        
        var encryptedClaims = GetTestEncryptedClaims();
        var token = _tokenUtility.GenerateToken(encryptedClaims);
        
        var (result, principal) = _tokenUtility.ValidateToken(token);
        
        Assert.Equal(TokenValidationResult.Valid, result);
        Assert.NotNull(principal);
    }
    
    [Fact]
    public async Task ValidateToken_WithExpiredToken_ShouldReturnExpiredResult()
    {
        // Adjust the expiration time to a very short period for this test
        _tokenUtility = new TokenUtility(_tokenValidationParameters, -1);

        var encryptedClaims = GetTestEncryptedClaims();
        var token = _tokenUtility.GenerateToken(encryptedClaims);

        // Wait for the token to expire
        await Task.Delay(2000);

        var (result, principal) = _tokenUtility.ValidateToken(token);
    
        Assert.Equal(TokenValidationResult.Expired, result);
        Assert.Null(principal);
    }
    
    [Fact]
    public void ValidateToken_WithInvalidToken_ShouldReturnInvalidResult()
    {
        _tokenUtility = new TokenUtility(_tokenValidationParameters, 1);

        // Generate a valid token first
        var encryptedClaims = GetTestEncryptedClaims();
        var token = _tokenUtility.GenerateToken(encryptedClaims);

        // Now tamper with the token to make it invalid
        var invalidToken = token + "tamper";

        // Validate the tampered token
        var (result, principal) = _tokenUtility.ValidateToken(invalidToken);

        Assert.Equal(TokenValidationResult.Invalid, result);
        Assert.Null(principal);
    }
    
    private string GetTestEncryptedClaims()
    {
        var claims = new TokenClaims
        {
            UserId = ObjectId.GenerateNewId(),
            Role = "User",
            SessionId = "",
            DeviceInfo = new DeviceInfo
            {
                DeviceId = "Lox",
                DeviceName = "Pidr",
                DeviceType = "4mo",
                OperatingSystem = "Mac",
                BrowserName = "Cheese",
                BrowserVersion = "00000",
                IpAddress = "hui"
            },
            LastModified = default
        }.ToString()!;
        return CryptoHelper.Encrypt(claims);
    }
}