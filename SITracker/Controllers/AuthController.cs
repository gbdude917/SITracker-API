using Microsoft.AspNetCore.Mvc;
using SITracker.DTOs;
using SITracker.Interfaces;

namespace SITracker.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthService _service;

        public AuthController(IUserAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                await _service.RegisterUserAsync(registerDto);
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
                var response = await _service.AuthenticateUserAsync(loginDto);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
