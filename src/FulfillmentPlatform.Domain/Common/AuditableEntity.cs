using System.ComponentModel.DataAnnotations;

namespace FulfillmentPlatform.Domain.Common;

public abstract class AuditableEntity
{
    public Guid Id { get; protected set; }

    public TenantId TenantId { get; protected set; } = default!;

    public DateTimeOffset CreatedAt { get; protected set; }

    public DateTimeOffset UpdatedAt { get; protected set; }

    [Timestamp]
    public byte[]? RowVersion { get; protected set; }

    protected void Touch() => UpdatedAt = DateTimeOffset.UtcNow;
}
