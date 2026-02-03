namespace Security.Application.Dtos.Results;


public record ProfilePaginationItem
(
    long Id,
    string Name,
    bool IsActive,
    long? ImageAttachmentId,
    string? ImageAttachmentUrl,
    DateTime? CreatedAt
);

public record ProfilePaginationResult
(
    IEnumerable<ProfilePaginationItem> Data,
    long TotalCount,
    int Page,
    int PageSize
);