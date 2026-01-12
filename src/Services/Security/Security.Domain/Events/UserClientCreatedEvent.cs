namespace Security.Domain.Events;

public record UserClientCreatedEvent(UserClient userClient) : IDomainEvent;
