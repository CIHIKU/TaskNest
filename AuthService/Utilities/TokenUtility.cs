using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Utilities;

public class TokenUtility : ITokenUtility
{
    private readonly string _secretKey;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly int _expirationInHours;

    public TokenUtility(TokenValidationParameters tokenValidationParameters, int expirationInHours)
    {
        _secretKey = Environment.GetEnvironmentVariable("SECRET_KEY") ?? "MySecretKey_1234MySecretKey_1234";
        
        _expirationInHours = expirationInHours;
        _tokenValidationParameters = tokenValidationParameters;
        _tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
    }

    public string GenerateToken(string encryptedClaims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var jwtToken = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(_expirationInHours),
            claims: new []
            {
                new Claim("TokenClaims", encryptedClaims)
            },
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.WriteToken(jwtToken);

        return securityToken;
    }

    public (TokenValidationResult result, ClaimsPrincipal? principal) ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);
            return (TokenValidationResult.Valid, principal);
        }
        catch (SecurityTokenExpiredException)
        {
            return (TokenValidationResult.Expired, null);
        }
        catch (Exception)
        {
            // Log other exceptions, as they will represent invalid tokens for reasons other than expiration
            return (TokenValidationResult.Invalid, null);
        }
    }
}