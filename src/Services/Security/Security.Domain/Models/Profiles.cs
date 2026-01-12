namespace Security.Domain.Models;

public class Profile: Aggregate<long>
{
    public string Name { get; set; } = "";
    public string? Code { get; set; }
    public bool IsActive { get; set; }
    public bool IsProtected { get; set; } = false;
    public long? ImageAttachmentId { get; set; }
    
    
    // Acceso 

    private readonly List<ProfileNavigationItem> _profileNavigationItems = new();
    public IReadOnlyList<ProfileNavigationItem> ProfileNavigationItems => _profileNavigationItems.AsReadOnly();
    
    public static Profile Create(long id, string name, bool isActive,  string? code, long? imageAttachmentId, bool isProtected)
    {
        var profile = new Profile
        {
            Name = name,
            IsActive = isActive,
            ImageAttachmentId = imageAttachmentId,
            Code = code,
            IsProtected = isProtected
        };

        profile.AddDomainEvent(new ProfileCreatedEvent(profile));
        return profile;
    }
    
    public void Update(string name, bool isActive, long? imageAttachmentId)
    {
        Name = name;
        IsActive = isActive;
        ImageAttachmentId = imageAttachmentId;

        AddDomainEvent(new ProfileUpdatedEvent(this));
    }

    public void SetNavigationItem(long navigationItemId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(navigationItemId);
        
        var menuItem = new ProfileNavigationItem(profileId: Id, navigationItemId: navigationItemId);
        _profileNavigationItems.Add(menuItem);
    }
    
    public void RemoveNavigationItem(long navigationItemId)
    {
        var navigationItem = _profileNavigationItems.FirstOrDefault(x => x.NavigationItemId == navigationItemId);
        if (navigationItem is not null)
        {
            _profileNavigationItems.Remove(navigationItem);
        }
    }
    
    public void HandleImageAttachment(long attachmentId)
    {
        ImageAttachmentId = attachmentId;
        
        AddDomainEvent(new ProfileUpdatedEvent(this));
    }
    
}