using FulfillmentPlatform.Application.Common;
using FulfillmentPlatform.Application.Orders.Commands;
using FulfillmentPlatform.Application.Orders.DTOs;
using FulfillmentPlatform.Application.Orders.Queries;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentPlatform.API.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    [HttpGet]
    public ActionResult<PaginatedResult<OrderDto>> List([FromQuery] ListOrdersQuery query)
    {
        var result = new PaginatedResult<OrderDto>(Array.Empty<OrderDto>(), query.PageNumber, query.PageSize, 0);
        return Ok(result);
    }

    [HttpGet("{orderId:guid}")]
    public ActionResult<OrderDto> Get(Guid orderId)
    {
        var query = new GetOrdersQuery(orderId);
        var dto = new OrderDto(query.OrderId, "demo-customer", "Pending", Array.Empty<OrderLineDto>());
        return Ok(dto);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand(request);
        return Accepted(new { command.Request.CustomerId, itemCount = command.Request.Items.Count });
    }

    [HttpPost("{orderId:guid}/confirm")]
    public IActionResult Confirm(Guid orderId)
    {
        var command = new ConfirmOrderCommand(orderId);
        return Accepted(command);
    }

    [HttpPost("{orderId:guid}/cancel")]
    public IActionResult Cancel(Guid orderId, [FromBody] CancelOrderBody body)
    {
        var command = new CancelOrderCommand(orderId, body.Reason);
        return Accepted(command);
    }

    public sealed record CancelOrderBody(string? Reason);
}
