using System.Security.Claims;
using AuthService.Models;
using AuthService.Models.Requests;
using AuthService.Models.Responses;
using AuthService.Repository;
using MongoDB.Bson;

namespace AuthService.Services;

public class TokenService : ITokenService
{
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ITokenRepository _tokenRepository;
    private readonly int _refreshTokenExpirationInMinutes;
    
    public TokenService(IJwtService jwtService, IRefreshTokenService refreshTokenService, ITokenRepository tokenRepository)
    {
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
        _tokenRepository = tokenRepository;
        _refreshTokenExpirationInMinutes = 360;
    }
    
    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string? ipAddress)
    {
        if (!await _refreshTokenService.ValidateTokenAsync(request.RefreshToken))
        {
            throw new Exception("Invalid refresh token");
        }
        
        var claimsPrincipal = _jwtService.ValidateToken(request.JwtToken);
        if (claimsPrincipal == null)
        {
            throw new Exception("Invalid JWT token");
        }
        
        var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var userId = ObjectId.Parse(userIdClaim);
        var roleClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (userIdClaim == null || roleClaim == null)
        {
            throw new Exception("JWT token does not contain necessary claims");
        }
        
        var newJwtToken = _jwtService.GenerateToken(userId, roleClaim);
        var newRefreshToken = _refreshTokenService.GenerateToken();
        
        var refreshTokenModel = new RefreshTokenModel
        {
            UserId = userId,
            Token = newRefreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(_refreshTokenExpirationInMinutes),
            CreatedByIp = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress))
        };

        await _tokenRepository.CreateAsync(refreshTokenModel);
        
        return new RefreshTokenResponse
        {
            JwtToken = newJwtToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task DeleteRefreshTokenAsync(string token, string ipAddress)
    {
        var refreshToken = await _tokenRepository.GetByTokenAsync(token);

        if (refreshToken == null) throw new Exception("Invalid token");

        await _tokenRepository.DeleteAsync(refreshToken.Id);
    }
}