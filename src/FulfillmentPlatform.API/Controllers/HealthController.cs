using Microsoft.AspNetCore.Mvc;

namespace FulfillmentPlatform.API.Controllers;

[ApiController]
[Route("health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new
    {
        status = "healthy",
        service = "FulfillmentPlatform.API",
        timestampUtc = DateTime.UtcNow
    });
}
