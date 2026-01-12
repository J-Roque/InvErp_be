namespace Security.Domain.Events;

public record PersonCreatedEvent(Person person): IDomainEvent;