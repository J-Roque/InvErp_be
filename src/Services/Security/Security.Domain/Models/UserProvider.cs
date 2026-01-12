

namespace Security.Domain.Models;
public class UserProvider : Aggregate<long>
{
    public long UserId { get; set; }
    public long ProviderId { get; set; }
    public long ProviderPersonId { get; set; }
    public bool IsActive { get; set; } = true;

    public static UserProvider Create(long userId, long providerId, long providerPersonId, bool isActive)
    {
        var userProvider = new UserProvider
        {
            UserId = userId,
            ProviderId = providerId,
            ProviderPersonId = providerPersonId,
            IsActive = isActive
        };

        userProvider.AddDomainEvent(new UserProviderCreatedEvent(userProvider));
        return userProvider;
    }

    public void Update(bool isActive)
    {
        IsActive = isActive;
        AddDomainEvent(new UserProviderUpdatedEvent(this));
    }

}
