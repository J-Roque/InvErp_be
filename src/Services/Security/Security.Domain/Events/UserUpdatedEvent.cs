namespace Security.Domain.Events;

public record UserUpdatedEvent(User user) : IDomainEvent;
