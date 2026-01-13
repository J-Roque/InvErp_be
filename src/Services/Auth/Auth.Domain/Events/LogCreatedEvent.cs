namespace Auth.Domain.Events;

public record LogCreatedEvent(AuthLog authLog) : IDomainEvent;
