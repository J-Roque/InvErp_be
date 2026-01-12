namespace Security.Domain.Events;

public record ProfileUpdatedEvent(Profile profile) : IDomainEvent;

