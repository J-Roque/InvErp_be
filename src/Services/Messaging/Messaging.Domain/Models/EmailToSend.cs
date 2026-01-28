using Messaging.Domain.Abstractions;
using Messaging.Domain.Events;

namespace Messaging.Domain.Models;

public class EmailToSend : Aggregate<long>
{
    public required string Subject { get; set; }
    public required string Template { get; set; }
    public string Data { get; set; } = "{}";
    public required string Recipients { get; set; }
    public string? Cc { get; set; }
    public string? Bcc { get; set; }
    public bool IsSent { get; set; } = false;
    public DateTime? SentDate { get; set; }
    public int Priority { get; set; } = 1;

    public static EmailToSend Create(string subject, string template, string data, string recipients, string? cc,
        string? bcc, int priority)
    {
        var email = new EmailToSend
        {
            Subject = subject,
            Template = template,
            Data = data,
            Recipients = recipients,
            Cc = cc,
            Bcc = bcc,
            Priority = priority
        };

        email.AddDomainEvent(new EmailToSendCreatedEvent(email));
        return email;
    }

    public void Sent()
    {
        IsSent = true;
        SentDate = DateTime.UtcNow;
        AddDomainEvent(new EmailToSendUpdatedEvent(this));
    }
}