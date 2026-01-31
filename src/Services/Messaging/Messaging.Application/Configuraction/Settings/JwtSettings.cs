namespace Messaging.Application.Configuraction.Settings;

public sealed class JwtSettings
{
    public string Secret { get; init; } = string.Empty;
    public int LoginExpirationMinutes { get; init; }
    public int PasswordResetExpirationMinutes { get; init; }
}
