namespace Security.Domain.Events;

public record UserClientUpdatedEvent(UserClient userClient) : IDomainEvent;