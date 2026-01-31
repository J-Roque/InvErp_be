
namespace Messaging.Application.Handlers.EmailsToSend.EventHandler;
public class EmailToSendUpdatedEventHandler(ILogger<EmailToSendUpdatedEventHandler> logger)
    : INotificationHandler<EmailToSendCreatedEvent>
{
    public Task Handle(EmailToSendCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}