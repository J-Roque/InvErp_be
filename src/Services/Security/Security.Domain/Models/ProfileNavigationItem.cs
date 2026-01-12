namespace Security.Domain.Models;

public class ProfileNavigationItem
{
    public long ProfileId { get; set; }
    public long NavigationItemId { get; set; }

    internal ProfileNavigationItem(long profileId, long navigationItemId)
    {
        ProfileId = profileId;
        NavigationItemId = navigationItemId;
    }

}