namespace FulfillmentPlatform.Domain.Common;

public record TenantId(Guid Value)
{
    public static TenantId New() => new(Guid.NewGuid());
    public static TenantId From(Guid value) => new(value);
    public static TenantId From(string value) => new(Guid.Parse(value));
    public override string ToString() => Value.ToString();
}
