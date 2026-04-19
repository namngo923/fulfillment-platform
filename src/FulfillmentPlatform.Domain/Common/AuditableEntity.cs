namespace FulfillmentPlatform.Domain.Common;

public abstract class AuditableEntity
{
    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; protected set; } = DateTime.UtcNow;

    protected void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}
