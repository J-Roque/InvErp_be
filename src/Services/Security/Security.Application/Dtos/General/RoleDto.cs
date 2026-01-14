using Security.Domain.Enums;

namespace Security.Application.Dtos.General;

public record RoleDto
(
    long? Id,
    string Name,
    string? Description,
    bool IsActive,
    long? ImageAttachmentId,
    string? ImageAttachmentUrl,
    IdentityStatus IdentityStatusId
);
