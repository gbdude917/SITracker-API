using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SITracker.Data;
using SITracker.DTOs;
using SITracker.Exceptions;
using SITracker.Interfaces;
using SITracker.Models;

namespace SITracker.Services
{
    public class UserService : IUserService
    {
        private readonly TrackerDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(TrackerDbContext context, IPasswordHasher<User> paswordHasher)
        {
            _context = context;
            _passwordHasher = paswordHasher;
        }

        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ActionResult<User>> GetUserById(long id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null) return new NotFoundResult();

            return user;
        }

        public async Task<ActionResult<User>> UpdateUsername(long id, UpdateUsernameDto updateUsernameDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null) return new NotFoundResult();

            if (string.IsNullOrEmpty(updateUsernameDto.NewUsername))
            {
                return new BadRequestObjectResult("New username cannot be null or empty.");
            }

            user.Username = updateUsernameDto.NewUserName;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ActionResult<User>> UpdatePassword(long id, UpdatePasswordDto updatePasswordDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null) return new NotFoundResult();

            if (string.IsNullOrEmpty(updatePasswordDto.OldPassword))
            {
                return new BadRequestObjectResult("Old password cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(updatePasswordDto.NewPassword))
            {
                return new BadRequestObjectResult("New password cannot be null or empty.");
            }

            // Ensure that old password matches password in DB
            if(user.Password != null && 
               _passwordHasher.VerifyHashedPassword(user, user.Password, updatePasswordDto.OldPassword) == PasswordVerificationResult.Failed)
            {
                throw new PasswordsDoNotMatchException("Passwords do not match!");
            }

            // TODO: Encode the new password and update accordingly
            user.Password = _passwordHasher.HashPassword(user, updatePasswordDto.NewPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null) return new NotFoundResult();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
