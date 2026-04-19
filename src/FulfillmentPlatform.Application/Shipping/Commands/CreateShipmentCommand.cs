namespace FulfillmentPlatform.Application.Shipping.Commands;

public sealed record CreateShipmentCommand(Guid OrderId, string Carrier);
