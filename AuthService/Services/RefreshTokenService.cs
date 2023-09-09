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
}