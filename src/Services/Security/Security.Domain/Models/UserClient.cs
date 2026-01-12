namespace Security.Domain.Models;

public class UserClient : Aggregate<long>
{
    public long UserId { get; set; }
    public long ClientId { get; set; }
    public long ClientPersonId { get; set; }
    public bool IsActive { get; set; } = true;

    public static UserClient Create(long userId, long clientId, long clientPersonId, bool isActive)
    {
        var userClient = new UserClient
        {
            UserId = userId,
            ClientId = clientId,
            ClientPersonId = clientPersonId,
            IsActive = isActive
        };

        userClient.AddDomainEvent(new UserClientCreatedEvent(userClient));
        return userClient;
    }

    public void Update(bool isActive)
    {
        IsActive = isActive;
        AddDomainEvent(new UserClientUpdatedEvent(this));
    }

}
