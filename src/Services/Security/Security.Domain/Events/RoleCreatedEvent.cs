namespace Security.Domain.Events
{
    public record RoleCreatedEvent(Role role) : IDomainEvent;
}
