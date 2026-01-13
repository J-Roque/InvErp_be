namespace Auth.Domain.Events;

public record LogUpdatedEvent(AuthLog authLog) : IDomainEvent;
