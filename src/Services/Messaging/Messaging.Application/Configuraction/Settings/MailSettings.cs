namespace Messaging.Application.Configuration.Settings;

public class MailSettings
{
    public string Host { get; init; } = "";
    public int Port { get; init; } = 587;
    public string From { get; init; } = "";
    public string User { get; init; } = "";
    public string Password { get; init; } = "";
    public bool EnableSsl { get; init; } = true;
}