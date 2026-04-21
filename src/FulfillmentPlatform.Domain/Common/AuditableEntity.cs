namespace FulfillmentPlatform.Domain.Common;

public abstract class AuditableEntity
{
    public Guid Id {set;get;}

    public TenantId TenantId{set;get;}

    public DateTimeOffset CreatedAt{set;get;}
    public DateTimeOffset CreatedAt{set;get;}

    [Timestamp]
    public byte[] RowVersion { get; set; }
}
