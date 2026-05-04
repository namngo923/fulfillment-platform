namespace FulfillmentPlatform.Domain.Orders;

public sealed class OrderItem
{
    public OrderItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0.", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid ProductId { get; }

    public int Quantity { get; }

    public decimal UnitPrice { get; }

    public decimal Total => Quantity * UnitPrice;
}
