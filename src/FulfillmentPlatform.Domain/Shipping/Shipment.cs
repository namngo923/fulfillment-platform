using FulfillmentPlatform.Domain.Common;

namespace FulfillmentPlatform.Domain.Shipping;

public sealed class Shipment : AuditableEntity
{
    public Shipment(Guid id, TenantId tenantId, Guid orderId, string carrier, string trackingCode)
    {
        Id = id;
        TenantId = tenantId;
        OrderId = orderId;
        Carrier = carrier;
        TrackingCode = trackingCode;
    }

    public Guid Id { get; }

    public TenantId TenantId { get; }

    public Guid OrderId { get; }

    public string Carrier { get; }

    public string TrackingCode { get; }
}
