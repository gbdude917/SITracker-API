using SITracker.Models;

namespace SITracker.Interfaces
{
    public interface IJwtService
    {
        string? GenerateToken(User user);
    }
}
