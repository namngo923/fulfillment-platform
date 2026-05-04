using FulfillmentPlatform.Domain.Orders;

namespace FulfillmentPlatform.Infrastructure.Persistence;

public sealed class AppDbContext
{
    public List<Order> Orders { get; } = new();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(0);
    }
}
