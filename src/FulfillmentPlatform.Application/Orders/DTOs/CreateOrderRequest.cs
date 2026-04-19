namespace FulfillmentPlatform.Application.Orders.DTOs;

public sealed class CreateOrderRequest
{
    public string CustomerId { get; init; } = string.Empty;

    public List<CreateOrderItemRequest> Items { get; init; } = new();
}

public sealed record CreateOrderItemRequest(Guid ProductId, int Quantity, decimal UnitPrice);
