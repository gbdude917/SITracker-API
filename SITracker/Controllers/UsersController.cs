using Microsoft.AspNetCore.Mvc;
using SITracker.DTOs;
using SITracker.Interfaces;
using SITracker.Models;

namespace SITracker.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            return await _service.GetAllUsers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(long id)
        {
            return await _service.GetUserById(id);
        }

        [HttpPatch("update-username/{id}")]
        public async Task<ActionResult<User>> UpdateUsername(long id, [FromBody] UpdateUsernameDto updateUsernameDto)
        {
            return await _service.UpdateUsername(id, updateUsernameDto);
        }

        [HttpPatch("update-password/{id}")]
        public async Task<ActionResult<User>> UpdatePassword(long id, [FromBody] UpdatePasswordDto updatePasswordDto)
        {
            return await _service.UpdatePassword(id, updatePasswordDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            return await _service.DeleteUser(id);
        }
    }
}
