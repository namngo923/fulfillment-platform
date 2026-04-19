using FulfillmentPlatform.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace FulfillmentPlatform.API.Controllers;

[ApiController]
[Route("api/tenants")]
public sealed class TenantsController(ICurrentTenant currentTenant) : ControllerBase
{
    [HttpGet("current")]
    public IActionResult Current() => Ok(new
    {
        tenantId = currentTenant.TenantId?.Value,
        correlationId = HttpContext.TraceIdentifier
    });
}
