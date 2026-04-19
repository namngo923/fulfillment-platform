using FulfillmentPlatform.Domain.Common;

namespace FulfillmentPlatform.Domain.Orders;

public sealed class Order : AuditableEntity
{
    private readonly List<OrderItem> _items = new();

    public Order(Guid id, TenantId tenantId, string customerId)
    {
        Id = id;
        TenantId = tenantId;
        CustomerId = customerId;
        Status = OrderStatus.Pending;
    }

    public Guid Id { get; }

    public TenantId TenantId { get; }

    public string CustomerId { get; }

    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public void AddItem(OrderItem item)
    {
        _items.Add(item);
        Touch();
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Only pending orders can be confirmed.");
        }

        Status = OrderStatus.Confirmed;
        Touch();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
        {
            throw new InvalidOperationException("Shipped orders cannot be cancelled.");
        }

        Status = OrderStatus.Cancelled;
        Touch();
    }
}
