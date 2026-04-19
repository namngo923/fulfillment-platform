namespace FulfillmentPlatform.Application.Orders.Commands;

public sealed record ConfirmOrderCommand(Guid OrderId);
