namespace Auth.Domain.Models;

public class Token: Aggregate<long>
{
    public required string Code { get; set; }
    public required string Type { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime LastUsedAt { get; set; }
    public string? Ip { get; set; }
    public bool IsActive { get; set; } = true;
    public long UserId { get; set; }
    public string? Device { get; set; }
    public string? UserAgent { get; set; }

    public static Token Create(long userId, string code, string type, DateTime generatedAt, DateTime expiresAt, DateTime lastUsedAt, string? ip, string? device, string? userAgent)
    {
        var token = new Token
        {
            UserId = userId,
            Code = code,
            Type = type,
            GeneratedAt = generatedAt,
            ExpiresAt = expiresAt,
            LastUsedAt = lastUsedAt,
            Ip = ip,
            IsActive = true,
            Device = device,
            UserAgent = userAgent
        };

        token.AddDomainEvent(new TokenCreatedEvent(token));
        return token;
    }

    public void UpdateLastUsedAt(DateTime lastUsedAt, long lastModifiedBy)
    {
        LastUsedAt = lastUsedAt;
        LastModified = DateTime.Now;
        LastModifiedBy = lastModifiedBy;

        AddDomainEvent(new TokenUpdatedEvent(this));
    }


}
