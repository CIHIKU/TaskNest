using System.Security.Cryptography;
using AuthService.Services.Interfaces;

namespace AuthService.Services;

public class RefreshTokenService : IRefreshTokenService
{
    public string GenerateToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    public async Task<bool> ValidateTokenAsync(string token)
    {
        // Here, you would typically check the token against a list of stored tokens in your database
        // For demonstration, I'm just returning true. Replace this with your actual validation logic.

        // Example:
        // var existingToken = await _dbContext.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == token);
        // return existingToken != null && existingToken.ExpiryDate > DateTime.UtcNow;

        return true;
    }
}