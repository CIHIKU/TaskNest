using System.Security.Claims;
using MongoDB.Bson;

namespace AuthService.Services;

public interface IJwtService
{
    public string GenerateToken(ObjectId userId, string role);
    public ClaimsPrincipal? ValidateToken(string token);
}