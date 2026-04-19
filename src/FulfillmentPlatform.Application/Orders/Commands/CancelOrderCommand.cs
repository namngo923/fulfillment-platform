namespace FulfillmentPlatform.Application.Orders.Commands;

public sealed record CancelOrderCommand(Guid OrderId, string? Reason);
