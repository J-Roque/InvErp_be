namespace Messaging.Domain.Utilities;

public class PaginationUtility
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool IgnorePagination { get; set; } = false;
    public string? Filters { get; set; } = null;
}
