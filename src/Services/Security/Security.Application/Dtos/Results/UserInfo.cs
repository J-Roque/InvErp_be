using System.Text.Json;

namespace Security.Application.Dtos.Results;

public class UserInfoSp
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int DocumentTypeId { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Email { get; set; }
    public string Username { get; set; } = "";
    public int IdentityStatusId { get; set; }
    public string? IdentityStatus { get; set; }
    public long? ImageAttachmentId { get; set; }
    public string? ImageAttachmentUrl { get; set; }
    public long ProfileId { get; set; }
    public string? ProfileName { get; set; }
    public string? RoleIds { get; set; }

    public DateTime? CreatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    public string? LastModifiedByName { get; set; }
}

public class UserInfo : UserInfoSp
{
    public new List<long>? RoleIds { get; set; }

    public UserInfo(UserInfoSp userInfoSp)
    {
        Id = userInfoSp.Id;
        FirstName = userInfoSp.FirstName;
        LastName = userInfoSp.LastName;
        DocumentTypeId = userInfoSp.DocumentTypeId;
        DocumentNumber = userInfoSp.DocumentNumber;
        Email = userInfoSp.Email;
        Username = userInfoSp.Username;
        IdentityStatusId = userInfoSp.IdentityStatusId;
        IdentityStatus = userInfoSp.IdentityStatus;
        ImageAttachmentId = userInfoSp.ImageAttachmentId;
        ImageAttachmentUrl = userInfoSp.ImageAttachmentUrl;
        ProfileId = userInfoSp.ProfileId;
        ProfileName = userInfoSp.ProfileName;

        RoleIds = userInfoSp.RoleIds != null
            ? JsonSerializer.Deserialize<List<long>>(userInfoSp.RoleIds)
            : [];

        CreatedAt = userInfoSp.CreatedAt;
        CreatedBy = userInfoSp.CreatedBy;
        CreatedByName = userInfoSp.CreatedByName;
        LastModified = userInfoSp.LastModified;
        LastModifiedBy = userInfoSp.LastModifiedBy;
        LastModifiedByName = userInfoSp.LastModifiedByName;
    }
}
