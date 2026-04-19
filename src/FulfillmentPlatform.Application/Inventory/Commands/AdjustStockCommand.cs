namespace FulfillmentPlatform.Application.Inventory.Commands;

public sealed record AdjustStockCommand(Guid InventoryItemId, int QuantityDelta, string Reason);
