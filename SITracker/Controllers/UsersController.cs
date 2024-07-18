using Microsoft.AspNetCore.Mvc;
using SITracker.Dtos;
using SITracker.Interfaces;
using SITracker.Models;

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

        [HttpPatch("update-username/{id}")]
        public async Task<ActionResult<User>> UpdateUsername(long id, [FromBody] UpdateUsernameDto updateUsernameDto)
        {
            return await _userService.UpdateUsername(id, updateUsernameDto);
        }

        [HttpPatch("update-password/{id}")]
        public async Task<ActionResult<User>> UpdatePassword(long id, [FromBody] UpdatePasswordDto updatePasswordDto)
        {
            return await _userService.UpdatePassword(id, updatePasswordDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            return await _userService.DeleteUser(id);
        }
    }
}
