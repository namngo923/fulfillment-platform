using FulfillmentPlatform.Application.Orders.DTOs;

namespace FulfillmentPlatform.Application.Orders.Commands;

public sealed record CreateOrderCommand(CreateOrderRequest Request);
