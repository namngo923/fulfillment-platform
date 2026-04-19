namespace FulfillmentPlatform.Domain.Orders;

public enum OrderStatus
{
    Draft = 0,
    Pending = 1,
    Confirmed = 2,
    Cancelled = 3,
    Shipped = 4
}
