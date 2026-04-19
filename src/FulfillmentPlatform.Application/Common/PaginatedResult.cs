namespace FulfillmentPlatform.Application.Common;

public sealed record PaginatedResult<T>(
    IReadOnlyCollection<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount);
