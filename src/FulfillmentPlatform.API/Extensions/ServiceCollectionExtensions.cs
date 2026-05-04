using FulfillmentPlatform.Application.Common;
using FulfillmentPlatform.Domain.Common;
using FulfillmentPlatform.Infrastructure.ExternalServices;
using FulfillmentPlatform.Infrastructure.Persistence;
using FulfillmentPlatform.Infrastructure.Repositories;

namespace FulfillmentPlatform.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPlatformServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICurrentTenant, CurrentTenantAccessor>();
        services.AddSingleton<AppDbContext>();
        services.AddSingleton<AppDbContextFactory>();
        services.AddSingleton<OrderRepository>();
        services.AddSingleton<IShippingProvider, MockShippingProvider>();

        return services;
    }

    private sealed class CurrentTenantAccessor : ICurrentTenant
    {
        public TenantId? TenantId { get; private set; }

        public void SetTenant(TenantId tenantId)
        {
            TenantId = tenantId;
        }
    }
}
