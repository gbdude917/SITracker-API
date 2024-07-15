using SITracker.DTOs;
using SITracker.Models;

namespace SITracker.Interfaces
{
    public interface IUserAuthService
    {
        Task<User> RegisterUserAsync(RegisterDto registerDto);

        Task<JwtResponse> AuthenticateUserAsync(LoginDto loginDto);
    }
}
