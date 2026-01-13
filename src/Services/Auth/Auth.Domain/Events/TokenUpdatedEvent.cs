namespace Auth.Domain.Events;

public record TokenUpdatedEvent(Token token) : IDomainEvent;
