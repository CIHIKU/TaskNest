using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuthService.Models;

public class RefreshTokenModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    
    [BsonElement("UserId")]
    public ObjectId UserId { get; set; }

    [BsonElement("Token")]
    public string Token { get; set; } = string.Empty;

    [BsonElement("Expiration")]
    public DateTime Expiration { get; set; }

    [BsonElement("Created")]
    public DateTime Created { get; set; } = DateTime.UtcNow;

    [BsonElement("CreatedByIp")]
    public string CreatedByIp { get; set; } = string.Empty;

    [BsonElement("Revoked")]
    public DateTime? Revoked { get; set; }

    [BsonElement("RevokedByIp")]
    public string? RevokedByIp { get; set; }

    [BsonElement("ReplacedByToken")]
    public string? ReplacedByToken { get; set; }
}