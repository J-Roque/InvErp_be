using Security.Domain.Enums;

namespace Security.Application.Dtos.Results;

public record RolePaginationItem
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
    DateTime? CreatedAt
);

public record RolePaginationResult
(
    IEnumerable<RolePaginationItem> Data,
    long TotalCount,
    int Page,
    int PageSize
);
