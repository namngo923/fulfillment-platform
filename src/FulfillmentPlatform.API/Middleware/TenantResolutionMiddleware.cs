using FulfillmentPlatform.Application.Common;
using FulfillmentPlatform.Domain.Common;

namespace FulfillmentPlatform.API.Middleware;

public sealed class TenantResolutionMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private const string HeaderName = "X-Tenant-Id";

    public async Task InvokeAsync(HttpContext context, ICurrentTenant currentTenant)
    {
        var rawTenantId = context.Request.Headers[HeaderName].FirstOrDefault()
            ?? configuration["FulfillmentPlatform:DefaultTenantId"];

        if (Guid.TryParse(rawTenantId, out var tenantId))
        {
            currentTenant.SetTenant(new TenantId(tenantId));
            context.Items[HeaderName] = tenantId;
        }

        await next(context);
    }
}
