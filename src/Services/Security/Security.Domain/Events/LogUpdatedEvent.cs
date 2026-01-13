namespace Security.Domain.Events;

public record LogUpdatedEvent(SecurityLog SecurityLog): IDomainEvent;
