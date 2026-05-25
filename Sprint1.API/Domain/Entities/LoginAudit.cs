using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sprint1.Domain.Entities;

/// <summary>
/// Entidade para auditoria de tentativas de login no MongoDB
/// </summary>
public class LoginAudit
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("email")]
    [BsonRequired]
    public string Email { get; set; } = string.Empty;

    [BsonElement("ipAddress")]
    public string? IpAddress { get; set; }

    [BsonElement("success")]
    [BsonRequired]
    public bool Success { get; set; }

    [BsonElement("timestamp")]
    [BsonRequired]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime Timestamp { get; set; }

    [BsonElement("failureReason")]
    public string? FailureReason { get; set; }

    [BsonElement("userId")]
    public int? UserId { get; set; }

    [BsonElement("userAgent")]
    public string? UserAgent { get; set; }
}

// Made with Bob