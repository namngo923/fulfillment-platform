using FulfillmentPlatform.Application.Inventory.Commands;
using FulfillmentPlatform.Application.Inventory.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentPlatform.API.Controllers;

[ApiController]
[Route("api/inventory")]
public sealed class InventoryController : ControllerBase
{
    [HttpGet("low-stock")]
    public IActionResult LowStock([FromQuery] int threshold = 10)
    {
        var query = new LowInventoryQuery(threshold);
        return Ok(new { query.Threshold, items = Array.Empty<object>() });
    }

    [HttpPost("reserve")]
    public IActionResult Reserve([FromBody] ReserveStockBody body)
    {
        var command = new ReserveStockCommand(body.OrderId, body.Items);
        return Accepted(command);
    }

    [HttpPost("adjust")]
    public IActionResult Adjust([FromBody] AdjustStockBody body)
    {
        var command = new AdjustStockCommand(body.InventoryItemId, body.QuantityDelta, body.Reason);
        return Accepted(command);
    }

    public sealed record ReserveStockBody(Guid OrderId, IReadOnlyCollection<ReservationLine> Items);
    public sealed record AdjustStockBody(Guid InventoryItemId, int QuantityDelta, string Reason);
}
