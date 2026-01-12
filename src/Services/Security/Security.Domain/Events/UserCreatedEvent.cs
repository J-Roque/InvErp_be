
namespace Security.Domain.Events
{
    public record UserCreatedEvent(User user) : IDomainEvent;

}
