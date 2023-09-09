using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SharedLibrary.Models;

public class UserModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public string Id { get; set; } = string.Empty;
    
    [BsonElement("Email")]
    [JsonPropertyName("email")]
    [EmailAddress]
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [BsonElement("PasswordHash")]
    [BsonIgnoreIfNull]
    [BsonIgnoreIfDefault]
    [JsonIgnore]
    public byte[]? PasswordHash { get; set; }
    
    [BsonElement("PasswordSalt")]
    [BsonIgnoreIfNull]
    [BsonIgnoreIfDefault]
    [JsonIgnore]
    public byte[]? PasswordSalt { get; set; }
    
    [BsonElement("Role")]
    [JsonPropertyName("role")]
    public string Role { get; set; } = "User";
    
    [BsonElement("OAuthIdentities")]
    [JsonPropertyName("oauthIdentities")]
    [BsonIgnoreIfDefault]
    [BsonIgnoreIfNull]
    public List<OAuthIdentity> OAuthIdentities { get; set; } = new List<OAuthIdentity>();
}