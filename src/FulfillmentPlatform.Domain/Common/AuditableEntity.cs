namespace FulfillmentPlatform.Domain.Common;
using System.ComponentModel.DataAnnotations;

public abstract class AuditableEntity
{
    public Guid Id { protected set; get;}

    public TenantId TenantId{ protected set; get;}
    public DateTimeOffset CreatedAt{ protected set; get;}
    public DateTimeOffset UpdatedAt { protected set; get;}

    [Timestamp]
    public byte[]? RowVersion { get; protected set; }
}
