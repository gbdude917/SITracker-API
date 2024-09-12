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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(long id)
        {
            // Get the user id from the jwt and check that it matches the id to be retrieved
            var userIdFromJwt = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdFromJwt == null || long.Parse(userIdFromJwt) != id)
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserById(id);

            if (user == null) { return NotFound(); }

            return Ok(user);
        }

        [Authorize]
        [HttpPatch("update-username/{id}")]
        public async Task<ActionResult<User>> UpdateUsername(long id, [FromBody] UpdateUsernameDto updateUsernameDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var oldUsername = User.FindFirstValue(ClaimTypes.Name);

            // Ensure user cannot update other user's credentials
            if (userId == null || long.Parse(userId) != id)
            {
                return Unauthorized();
            }

            // Ensure that user cannot update their username to their old username
            if (oldUsername == updateUsernameDto.NewUsername)
            {
                return BadRequest("The new username cannot be the same as the old username.");
            }

            var updatedUser =  await _userService.UpdateUsername(id, updateUsernameDto);

            return updatedUser;
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
