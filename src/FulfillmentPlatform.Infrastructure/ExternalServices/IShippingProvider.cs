namespace FulfillmentPlatform.Infrastructure.ExternalServices;

public interface IShippingProvider
{
    Task<string> CreateTrackingCodeAsync(Guid orderId, CancellationToken cancellationToken = default);
}
