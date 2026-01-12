namespace Security.Domain.Events;

public record PersonUpdatedEvent(Person person): IDomainEvent;