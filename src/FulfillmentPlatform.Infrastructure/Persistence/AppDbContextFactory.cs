namespace FulfillmentPlatform.Infrastructure.Persistence;

public sealed class AppDbContextFactory
{
    private readonly AppDbContext _context;

    public AppDbContextFactory(AppDbContext context)
    {
        _context = context;
    }

    public AppDbContext CreateDbContext() => _context;
}
