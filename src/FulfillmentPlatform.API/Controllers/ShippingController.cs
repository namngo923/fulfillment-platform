using FulfillmentPlatform.Application.Shipping.Commands;
using FulfillmentPlatform.Application.Shipping.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentPlatform.API.Controllers;

[ApiController]
[Route("api/shipping")]
public sealed class ShippingController : ControllerBase
{
    [HttpGet("{shipmentId:guid}")]
    public IActionResult Get(Guid shipmentId)
    {
        var query = new GetShipmentQuery(shipmentId);
        return Ok(new { query.ShipmentId, carrier = "mock-carrier", trackingCode = "TRK-DEMO" });
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateShipmentBody body)
    {
        var command = new CreateShipmentCommand(body.OrderId, body.Carrier);
        return Accepted(command);
    }

    public sealed record CreateShipmentBody(Guid OrderId, string Carrier);
}
