namespace AuthService.Services;

public interface IRefreshTokenService
{
    public string GenerateToken();
    public Task<bool> ValidateTokenAsync(string token);
}