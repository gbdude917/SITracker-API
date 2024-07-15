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
    public class UserService : IUserService, IUserAuthService
    {
        private readonly TrackerDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwtService;

        public UserService(TrackerDbContext context, IPasswordHasher<User> paswordHasher, IJwtService jwtService)
        {
            _context = context;
            _passwordHasher = paswordHasher;
            _jwtService = jwtService;
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

            user.Username = updateUsernameDto.NewUsername;

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

        public async Task<User> RegisterUserAsync(RegisterDto registerDto)
        {
            // Check if username or emails exists in DB
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username || u.Email == registerDto.Email))
            {
                throw new Exception("Username or email already exists");
            }

            if (registerDto.Username == null || registerDto.Email == null || registerDto.Password == null)
            {
                throw new Exception("Register detail fields cannot be null.");
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = _passwordHasher.HashPassword(new User(), registerDto.Password),
                RegistrationDate = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<JwtResponse> AuthenticateUserAsync(LoginDto loginDto)
        {
            if (loginDto.Username == null || loginDto.Password == null)
            {
                throw new Exception("Username or password cannot be empty when logging in.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);

            // Check if user has been found
            if (user == null)
            {
                throw new Exception("Username or password is incorrect.");
            }

            // Ensure password exists
            if (user.Password == null)
            {
                throw new Exception("The user object has an invalid/non-existent password");
            }
            
            // Verify that the passwords match
            if (_passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password) == PasswordVerificationResult.Failed)
            {
                throw new Exception("Username or password is incorrect.");
            }

            // Authenticate the user
            var token = _jwtService.GenerateToken(user);

            return new JwtResponse
            {
                Token = token,
                Username = user.Username,
                Expiration = DateTime.UtcNow.AddMinutes(30)
            };
        }
    }
}
