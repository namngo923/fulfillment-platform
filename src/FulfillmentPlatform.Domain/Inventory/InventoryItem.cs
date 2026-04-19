using FulfillmentPlatform.Domain.Common;

namespace FulfillmentPlatform.Domain.Inventory;

public sealed class InventoryItem : AuditableEntity
{
    public InventoryItem(Guid id, TenantId tenantId, Guid productId, int onHand)
    {
        Id = id;
        TenantId = tenantId;
        ProductId = productId;
        OnHand = onHand;
    }

    public Guid Id { get; }

    public TenantId TenantId { get; }

    public Guid ProductId { get; }

    public int OnHand { get; private set; }

    public void Reserve(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity));
        }

        if (quantity > OnHand)
        {
            throw new InvalidOperationException("Insufficient stock.");
        }

        OnHand -= quantity;
        Touch();
    }

    public void Adjust(int delta)
    {
        OnHand += delta;
        Touch();
    }
}
