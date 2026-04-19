namespace FulfillmentPlatform.Application.Orders.Queries;

public sealed record ListOrdersQuery(int PageNumber = 1, int PageSize = 20, string? Status = null);
