namespace Security.Domain.Models;

public class SecurityLog: Aggregate<long>
{
    public required string Content { get; set; } = "";

    public static SecurityLog Create(string content)
    {
        var log = new SecurityLog()
        {
            Content = content
        };

        log.AddDomainEvent(new LogCreatedEvent(log));
        return log;
    }

    public void Update(string content)
    {
        Content = content;
        AddDomainEvent(new LogUpdatedEvent(this));
    }
}
