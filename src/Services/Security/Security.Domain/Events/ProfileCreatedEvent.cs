namespace Security.Domain.Events;

public record ProfileCreatedEvent(Profile profile) : IDomainEvent;