using Microsoft.AspNetCore.Mvc;
using SITracker.DTOs;
using SITracker.Models;

namespace SITracker.Interfaces
{
    public interface IUserService
    {
        Task<ActionResult<List<User>>> GetAllUsers();

        Task<ActionResult<User>> GetUserById(long id);

        Task<ActionResult<User>> UpdateUsername(long id, UpdateUsernameDto usernameDto);

        Task<ActionResult<User>> UpdatePassword(long id, UpdatePasswordDto usernameDto);

        Task<ActionResult> DeleteUser(long id);
    }
}
