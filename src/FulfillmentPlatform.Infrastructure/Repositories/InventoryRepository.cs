using FulfillmentPlatform.Domain.Inventory;
using FulfillmentPlatform.Infrastructure.Persistence;

namespace FulfillmentPlatform.Infrastructure.Repositories;

public sealed class InventoryRepository(AppDbContext dbContext)
{
    public Task<InventoryItem?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var item = dbContext.InventoryItems.FirstOrDefault(entry => entry.ProductId == productId);
        return Task.FromResult(item);
    }

    public Task SaveAsync(InventoryItem item, CancellationToken cancellationToken = default)
    {
        if (!dbContext.InventoryItems.Any(entry => entry.Id == item.Id))
        {
            dbContext.InventoryItems.Add(item);
        }

        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
