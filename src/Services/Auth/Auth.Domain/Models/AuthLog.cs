namespace Auth.Domain.Models;

public class AuthLog: Aggregate<long>
{
    public required string Content { get; set; } = "";

    public static AuthLog Create(string content)
    {
        var log = new AuthLog()
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
