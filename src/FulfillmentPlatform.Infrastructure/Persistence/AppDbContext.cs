using FulfillmentPlatform.Domain.Inventory;
using FulfillmentPlatform.Domain.Orders;
using FulfillmentPlatform.Domain.Shipping;

namespace FulfillmentPlatform.Infrastructure.Persistence;

public sealed class AppDbContext
{
    public List<Order> Orders { get; } = new();

    public List<InventoryItem> InventoryItems { get; } = new();

    public List<Shipment> Shipments { get; } = new();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(0);
    }
}
