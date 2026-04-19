namespace FulfillmentPlatform.Application.Inventory.Queries;

public sealed record LowInventoryQuery(int Threshold = 10);
