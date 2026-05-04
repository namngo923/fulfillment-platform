using FulfillmentPlatform.Domain.Common;
using FulfillmentPlatform.Domain.Orders;
using Xunit;

namespace FulfillmentPlatform.Tests.Unit.Orders;

public class OrderStateMachineTests
{
    private static Order NewPendingOrder() =>
        new(Guid.NewGuid(), TenantId.New(), customerId: "customer-1");

    private static Order OrderAt(OrderStatus target)
    {
        var order = NewPendingOrder();
        if (target == OrderStatus.Pending) return order;
        order.Confirm();
        if (target == OrderStatus.Confirmed) return order;
        order.StartPicking();
        if (target == OrderStatus.Picking) return order;
        order.Pack();
        if (target == OrderStatus.Packed) return order;
        order.Ship("TRACK-001");
        if (target == OrderStatus.Shipped) return order;
        order.Deliver();
        return order; // Delivered
    }

    // ── Initial state ─────────────────────────────────────────────────────────

    [Fact]
    public void NewOrder_StartsAsPending_WithNoItems()
    {
        var order = NewPendingOrder();

        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Empty(order.Items);
        Assert.Null(order.TrackingNumber);
    }

    [Fact]
    public void NewOrder_ExposesConstructorValues()
    {
        var id = Guid.NewGuid();
        var tenantId = TenantId.New();

        var order = new Order(id, tenantId, "cust-42");

        Assert.Equal(id, order.Id);
        Assert.Equal(tenantId, order.TenantId);
        Assert.Equal("cust-42", order.CustomerId);
    }

    // ── AddItem ───────────────────────────────────────────────────────────────

    [Fact]
    public void AddItem_WhenPending_AppendsToItems()
    {
        var order = NewPendingOrder();
        var item = new OrderItem(Guid.NewGuid(), quantity: 2, unitPrice: 9.99m);

        order.AddItem(item);

        Assert.Single(order.Items);
    }

    [Fact]
    public void AddItem_WhenConfirmed_Throws()
    {
        var order = OrderAt(OrderStatus.Confirmed);

        Assert.Throws<InvalidOperationException>(() =>
            order.AddItem(new OrderItem(Guid.NewGuid(), 1, 5m)));
    }

    // ── Confirm ───────────────────────────────────────────────────────────────

    [Fact]
    public void Confirm_WhenPending_TransitionsToConfirmed()
    {
        var order = NewPendingOrder();

        order.Confirm();

        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Theory]
    [InlineData(OrderStatus.Confirmed)]
    [InlineData(OrderStatus.Picking)]
    [InlineData(OrderStatus.Packed)]
    [InlineData(OrderStatus.Shipped)]
    [InlineData(OrderStatus.Delivered)]
    public void Confirm_WhenNotPending_Throws(OrderStatus startStatus)
    {
        var order = OrderAt(startStatus);

        Assert.Throws<InvalidOperationException>(() => order.Confirm());
    }

    // ── StartPicking ──────────────────────────────────────────────────────────

    [Fact]
    public void StartPicking_WhenConfirmed_TransitionsToPicking()
    {
        var order = OrderAt(OrderStatus.Confirmed);

        order.StartPicking();

        Assert.Equal(OrderStatus.Picking, order.Status);
    }

    [Theory]
    [InlineData(OrderStatus.Pending)]
    [InlineData(OrderStatus.Picking)]
    [InlineData(OrderStatus.Packed)]
    public void StartPicking_WhenNotConfirmed_Throws(OrderStatus startStatus)
    {
        var order = OrderAt(startStatus);

        Assert.Throws<InvalidOperationException>(() => order.StartPicking());
    }

    // ── Pack ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Pack_WhenPicking_TransitionsToPacked()
    {
        var order = OrderAt(OrderStatus.Picking);

        order.Pack();

        Assert.Equal(OrderStatus.Packed, order.Status);
    }

    [Theory]
    [InlineData(OrderStatus.Pending)]
    [InlineData(OrderStatus.Confirmed)]
    [InlineData(OrderStatus.Packed)]
    public void Pack_WhenNotPicking_Throws(OrderStatus startStatus)
    {
        var order = OrderAt(startStatus);

        Assert.Throws<InvalidOperationException>(() => order.Pack());
    }

    // ── Ship ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Ship_WhenPacked_TransitionsToShipped_AndSetsTrackingNumber()
    {
        var order = OrderAt(OrderStatus.Packed);

        order.Ship("TRACK-123");

        Assert.Equal(OrderStatus.Shipped, order.Status);
        Assert.Equal("TRACK-123", order.TrackingNumber);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Ship_WithBlankTrackingNumber_Throws(string trackingNumber)
    {
        var order = OrderAt(OrderStatus.Packed);

        Assert.Throws<ArgumentException>(() => order.Ship(trackingNumber));
    }

    [Theory]
    [InlineData(OrderStatus.Pending)]
    [InlineData(OrderStatus.Confirmed)]
    [InlineData(OrderStatus.Picking)]
    public void Ship_WhenNotPacked_Throws(OrderStatus startStatus)
    {
        var order = OrderAt(startStatus);

        Assert.Throws<InvalidOperationException>(() => order.Ship("TRACK-001"));
    }

    // ── Deliver ───────────────────────────────────────────────────────────────

    [Fact]
    public void Deliver_WhenShipped_TransitionsToDelivered()
    {
        var order = OrderAt(OrderStatus.Shipped);

        order.Deliver();

        Assert.Equal(OrderStatus.Delivered, order.Status);
    }

    [Theory]
    [InlineData(OrderStatus.Pending)]
    [InlineData(OrderStatus.Confirmed)]
    [InlineData(OrderStatus.Picking)]
    [InlineData(OrderStatus.Packed)]
    public void Deliver_WhenNotShipped_Throws(OrderStatus startStatus)
    {
        var order = OrderAt(startStatus);

        Assert.Throws<InvalidOperationException>(() => order.Deliver());
    }

    // ── Cancel ───────────────────────────────────────────────────────────────

    [Theory]
    [InlineData(OrderStatus.Pending)]
    [InlineData(OrderStatus.Confirmed)]
    [InlineData(OrderStatus.Picking)]
    [InlineData(OrderStatus.Packed)]
    public void Cancel_WhenCancellable_TransitionsToCancelled(OrderStatus startStatus)
    {
        var order = OrderAt(startStatus);

        order.Cancel();

        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Theory]
    [InlineData(OrderStatus.Shipped)]
    [InlineData(OrderStatus.Delivered)]
    public void Cancel_WhenShippedOrDelivered_Throws(OrderStatus startStatus)
    {
        var order = OrderAt(startStatus);

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_Throws()
    {
        var order = NewPendingOrder();
        order.Cancel();

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }
}
