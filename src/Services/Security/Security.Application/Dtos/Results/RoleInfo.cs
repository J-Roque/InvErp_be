using Security.Domain.Enums;

namespace Security.Application.Dtos.Results;

public record RoleInfo
(
    long Id,
    string Name,
    string? Code,
    string? Description,
    bool IsActive,
    bool IsProtected,
    long? ImageAttachmentId,
    string? ImageAttachmentUrl,
    IdentityStatus IdentityStatusId,
    string? IdentityStatus,
    DateTime? CreatedAt,
    long? CreatedBy,
    string? CreatedByName,
    DateTime? LastModified,
    long? LastModifiedBy,
    string? LastModifiedByName
);
