namespace Auth.Application.Configuration.Settings;

public sealed class BackendSettings
{
    public string Auth { get; init; } = string.Empty;
    public string Security { get; init; } = string.Empty;
    public string Messaging { get; init; } = string.Empty;
    public string Attachment { get; init; } = string.Empty;
    public string Event { get; init; } = string.Empty;
}
