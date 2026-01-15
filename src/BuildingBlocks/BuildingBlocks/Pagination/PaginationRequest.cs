namespace BuildingBlocks.Pagination;

public record PaginationRequest(int Page = 1, int PageSize = 10, int IgnorePagination = 0, string? SearchTerm = null);
