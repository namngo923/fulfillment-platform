using Microsoft.AspNetCore.Mvc;

namespace FulfillmentPlatform.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    [HttpPost("token")]
    public IActionResult IssueToken([FromBody] LoginRequest request) => Ok(new
    {
        token = $"demo-token-for-{request.Username}",
        expiresInSeconds = 3600
    });

    public sealed record LoginRequest(string Username, string Password);
}
