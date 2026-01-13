namespace Security.Domain.Events;

public record LogCreatedEvent(SecurityLog SecurityLog): IDomainEvent;
