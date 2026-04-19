using FulfillmentPlatform.Domain.Orders;
using FulfillmentPlatform.Infrastructure.Persistence;

namespace FulfillmentPlatform.Infrastructure.Repositories;

public sealed class OrderRepository(AppDbContext dbContext)
{
    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        dbContext.Orders.Add(order);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = dbContext.Orders.FirstOrDefault(item => item.Id == orderId);
        return Task.FromResult(order);
    }
}
