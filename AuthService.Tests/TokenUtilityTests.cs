using AuthService.Helpers;
using AuthService.Utilities;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using TokenValidationResult = AuthService.Utilities.TokenValidationResult;

namespace AuthService.Tests;

public class TokenUtilityTests
{
    private readonly TokenUtility _tokenUtility;

    public TokenUtilityTests()
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = null,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        
        _tokenUtility = new TokenUtility(tokenValidationParameters, 1);
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken()
    {
        var encryptedClaims = GetTestEncryptedClaims();
        var token = _tokenUtility.GenerateToken(encryptedClaims);
        
        Assert.NotNull(token);
        Assert.IsType<string>(token);
    }
    
    [Fact]
    public void ValidateToken_WithValidToken_ShouldReturnValidResult()
    {
        var encryptedClaims = GetTestEncryptedClaims();
        var token = _tokenUtility.GenerateToken(encryptedClaims);
        
        var (result, principal) = _tokenUtility.ValidateToken(token);
        
        Assert.Equal(TokenValidationResult.Valid, result);
        Assert.NotNull(principal);
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