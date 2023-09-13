namespace AuthService.Models.Responses;

public class RefreshTokenResponse
{
    public required string RefreshToken { get; set; }
    public required string JwtToken { get; set; }
}