namespace AuthService.Services.Interfaces;

public interface IJwtService
{
    public string GenerateToken(string userId, string role);
}