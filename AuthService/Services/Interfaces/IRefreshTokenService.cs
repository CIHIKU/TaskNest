namespace AuthService.Services.Interfaces;

public interface IRefreshTokenService
{
    public string GenerateToken();
}