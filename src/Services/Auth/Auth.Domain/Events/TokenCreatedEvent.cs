namespace Auth.Domain.Events;

public record TokenCreatedEvent(Token token) : IDomainEvent;
