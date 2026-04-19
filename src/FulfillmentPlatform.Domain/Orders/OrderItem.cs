namespace FulfillmentPlatform.Domain.Orders;

public sealed class OrderItem
{
    public OrderItem(Guid productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public Guid ProductId { get; }

    public int Quantity { get; }

    public decimal UnitPrice { get; }
}
