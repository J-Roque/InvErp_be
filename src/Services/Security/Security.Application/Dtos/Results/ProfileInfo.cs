using System.Text.Json;

namespace Security.Application.Dtos.Results;

public class ProfileInfoSp
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public long? ImageAttachmentId { get; set; }
    public string? ImageAttachmentUrl { get; set; }
    public string? NavigationItemIds { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    public string? LastModifiedByName { get; set; }
}

public class ProfileInfo : ProfileInfoSp
{
    public new List<long>? NavigationItemIds { get; set; }

    public ProfileInfo(ProfileInfoSp profileInfoSp)
    {
        Id = profileInfoSp.Id;
        Name = profileInfoSp.Name;
        IsActive = profileInfoSp.IsActive;
        ImageAttachmentId = profileInfoSp.ImageAttachmentId;
        ImageAttachmentUrl = profileInfoSp.ImageAttachmentUrl;
        NavigationItemIds = profileInfoSp.NavigationItemIds != null
            ? JsonSerializer.Deserialize<List<long>>(profileInfoSp.NavigationItemIds)
            : [];
        
        CreatedAt = profileInfoSp.CreatedAt;
        CreatedBy = profileInfoSp.CreatedBy;
        CreatedByName = profileInfoSp.CreatedByName;
        LastModified = profileInfoSp.LastModified;
        LastModifiedBy = profileInfoSp.LastModifiedBy;
        LastModifiedByName = profileInfoSp.LastModifiedByName;
    }
}