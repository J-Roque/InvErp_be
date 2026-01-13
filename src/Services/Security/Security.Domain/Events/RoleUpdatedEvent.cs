namespace Security.Domain.Events
{
    public record RoleUpdatedEvent(Role role) : IDomainEvent;
}
