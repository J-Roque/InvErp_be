
namespace Security.Application.Dtos.Results
{
    public record UserPaginationItem
    (
        long Id,
        string Username,
        string FirstName,
        string LastName,
        string Email,
        int IdentityStatusId,
        string IdentityStatus,
        long? ProfileId,
        string? ProfileName,
        long? ImageAttachmentId,
        string? ImageAttachmentUrl,
        DateTime? CreatedAt
    );

    public record UserPaginationResult
    (
        IEnumerable<UserPaginationItem> Data,
        long TotalCount,
        int Page,
        int PageSize
    );
}
