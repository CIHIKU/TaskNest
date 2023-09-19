using System.Security.Claims;
using MongoDB.Bson;

namespace AuthService.Utilities;

public interface ITokenUtility
{
    string GenerateToken(string encryptedClaims);
    (TokenValidationResult result, ClaimsPrincipal? principal) ValidateToken(string token);
}

public class DeviceInfo
{
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public string DeviceType { get; set; } // e.g., Mobile, Tablet, Desktop
    public string OperatingSystem { get; set; }
    public string BrowserName { get; set; }
    public string BrowserVersion { get; set; }
    public string IpAddress { get; set; }
}
    
public class TokenClaims
{
    public ObjectId UserId { get; set; }
    public string Role { get; set; }
    public string SessionId { get; set; }
    public DeviceInfo DeviceInfo { get; set; }
    public DateTime LastModified { get; set; }
}

public enum TokenValidationResult
{
    Valid,
    Expired,
    Invalid
}