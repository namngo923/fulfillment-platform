namespace FulfillmentPlatform.Application.Inventory.Commands;

public sealed record ReserveStockCommand(Guid OrderId, IReadOnlyCollection<ReservationLine> Items);

public sealed record ReservationLine(Guid ProductId, int Quantity);
