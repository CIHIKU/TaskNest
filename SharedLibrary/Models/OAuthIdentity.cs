using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace SharedLibrary.Models;

public abstract class OAuthIdentity
{
    [BsonElement("Provider")]
    [JsonPropertyName("provider")]
    [Required]
    public string Provider { get; set; } = string.Empty;

    [BsonElement("ProviderId")]
    [JsonPropertyName("providerId")]
    [Required]
    public string ProviderId { get; set; } = string.Empty;
}