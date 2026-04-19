namespace FulfillmentPlatform.Infrastructure.ExternalServices;

public sealed class MockShippingProvider : IShippingProvider
{
    public Task<string> CreateTrackingCodeAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult($"TRK-{orderId:N}"[..12]);
    }
}
