namespace Security.Domain.Events;

public record UserProviderCreatedEvent(UserProvider UserProvider) : IDomainEvent;

