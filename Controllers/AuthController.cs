using FacebookLike.Service.Security;
using Microsoft.AspNetCore.Mvc;
using FacebookLike.Models;

namespace FacebookLike.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await authService.Authenticate(model.Username, model.Password);
        if (user == null)
            return Unauthorized(new { success = false, message = "Invalid credentials" });
        return Ok(new { success = true, user });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        authService.Logout();
        return Ok(new { success = true });
    }
} 