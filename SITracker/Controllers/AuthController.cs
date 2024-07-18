using Microsoft.AspNetCore.Mvc;
using SITracker.Dtos;
using SITracker.Interfaces;

namespace SITracker.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthService _authService;

        public AuthController(IUserAuthService service)
        {
            _authService = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                await _authService.RegisterUserAsync(registerDto);
                return Ok("User registered successfully!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var response = await _authService.AuthenticateUserAsync(loginDto);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
