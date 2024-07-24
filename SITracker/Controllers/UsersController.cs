using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SITracker.Dtos;
using SITracker.Interfaces;
using SITracker.Models;
using System.Security.Claims;

namespace SITracker.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService service)
        {
            _userService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            return await _userService.GetAllUsers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(long id)
        {
            return await _userService.GetUserById(id);
        }

        [Authorize]
        [HttpPatch("update-username/{id}")]
        public async Task<ActionResult<User>> UpdateUsername(long id, [FromBody] UpdateUsernameDto updateUsernameDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure user cannot update other user's credentials
            if (userId == null || long.Parse(userId) != id)
            {
                return Unauthorized();
            }

            return await _userService.UpdateUsername(id, updateUsernameDto);
        }

        [Authorize]
        [HttpPatch("update-password/{id}")]
        public async Task<ActionResult<User>> UpdatePassword(long id, [FromBody] UpdatePasswordDto updatePasswordDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure user cannot update other user's credentials
            if (userId == null || long.Parse(userId) != id)
            {
                return Unauthorized();
            }

            var result = await _userService.UpdatePassword(id, updatePasswordDto);

            return Ok(new { Message = "Password updated. Please log in again." });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            return await _userService.DeleteUser(id);
        }
    }
}
