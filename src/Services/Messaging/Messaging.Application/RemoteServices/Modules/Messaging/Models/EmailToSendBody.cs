namespace Messaging.Application.RemoteServices.Modules.Messaging.Models;

public class EmailToSendBody
{
    public string Subject { get; set; } = string.Empty;
    public string Template { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string Recipients { get; set; } = string.Empty;
    public string Cc { get; set; } = string.Empty;
    public string Bcc { get; set; } = string.Empty;
    public int Priority { get; set; }
}

public class SaveEmailToSendResult
{
    public long Id { get; set; }
}

public class SendEmailResult
{
    public bool IsSuccess { get; set; }
}
