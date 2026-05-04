namespace FulfillmentPlatform.Domain.Orders;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Picking = 2,
    Packed = 3,
    Shipped = 4,
    Delivered = 5,
    Cancelled = 99
}