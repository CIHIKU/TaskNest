namespace AuthService.Models.Requests;

public class RefreshTokenRequest
{
    public required string RefreshToken { get; set; }
    public required string JwtToken { get; set; }
}