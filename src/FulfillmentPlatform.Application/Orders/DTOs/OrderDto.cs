namespace FulfillmentPlatform.Application.Orders.DTOs;

public sealed record OrderDto(
    Guid OrderId,
    string CustomerId,
    string Status,
    IReadOnlyCollection<OrderLineDto> Items);

public sealed record OrderLineDto(Guid ProductId, int Quantity, decimal UnitPrice);
