using FulfillmentPlatform.Domain.Common;

namespace FulfillmentPlatform.Application.Common;

public interface ICurrentTenant
{
    TenantId? TenantId { get; }

    void SetTenant(TenantId tenantId);
}
