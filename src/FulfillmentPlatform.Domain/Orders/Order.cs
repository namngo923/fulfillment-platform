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
        CreatedAt = DateTimeOffset.UtcNow;
    }
    public string CustomerId { get; }
    public OrderStatus Status { get; private set; }
    public string? TrackingNumber { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public void AddItem(OrderItem item)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Can only add items to pending orders.");

        _items.Add(item);
        Touch();
    }


    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot confirm order in {Status} status.");

        Status = OrderStatus.Confirmed;
        Touch();
    }

    public void StartPicking()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException($"Cannot start picking order in {Status} status.");

        Status = OrderStatus.Picking;
        Touch();
    }

    public void Pack()
    {
        if (Status != OrderStatus.Picking)
            throw new InvalidOperationException($"Cannot pack order in {Status} status.");

        Status = OrderStatus.Packed;
        Touch();
    }

    public void Ship(string trackingNumber)
    {
        if (Status != OrderStatus.Packed)
            throw new InvalidOperationException($"Cannot ship order in {Status} status.");

        if (string.IsNullOrWhiteSpace(trackingNumber))
            throw new ArgumentException("Tracking number is required.", nameof(trackingNumber));

        Status = OrderStatus.Shipped;
        TrackingNumber = trackingNumber;
        Touch();
    }

    public void Deliver()
    {
        if (Status != OrderStatus.Shipped)
            throw new InvalidOperationException($"Cannot deliver order in {Status} status.");

        Status = OrderStatus.Delivered;
        Touch();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped || Status == OrderStatus.Delivered || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel order in {Status} status.");

        Status = OrderStatus.Cancelled;
        Touch();
    }
    protected void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
