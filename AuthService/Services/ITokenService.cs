using AuthService.Models.Requests;
using AuthService.Models.Responses;

namespace AuthService.Services;

public interface ITokenService
{
    public Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string? ipAddress);

    public Task DeleteRefreshTokenAsync(string token, string ipAddress);
}