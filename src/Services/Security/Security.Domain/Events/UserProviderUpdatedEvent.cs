namespace Security.Domain.Events;

public record UserProviderUpdatedEvent(UserProvider userProvider) : IDomainEvent;