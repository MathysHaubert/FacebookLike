using System;
using Microsoft.AspNetCore.Mvc;
using FacebookLike.Services;
using FacebookLike.Models;

namespace FacebookLike.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _authService.Authenticate(request.Username, request.Password);

            if (user == null)
                return Unauthorized(new { message = "Nom d'utilisateur ou mot de passe incorrect" });

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
} 