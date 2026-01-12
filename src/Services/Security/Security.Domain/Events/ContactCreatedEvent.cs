namespace Security.Domain.Events;

public record ContactCreatedEvent(Contact contact) : IDomainEvent;
