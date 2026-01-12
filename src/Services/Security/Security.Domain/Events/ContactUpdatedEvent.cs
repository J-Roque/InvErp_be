namespace Security.Domain.Events;

public record ContactUpdatedEvent(Contact contact) : IDomainEvent;
