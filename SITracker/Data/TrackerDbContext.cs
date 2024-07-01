using Microsoft.EntityFrameworkCore;
using SITracker.Models;

namespace SITracker.Data
{
    public class TrackerDbContext : DbContext
    {
        public TrackerDbContext(DbContextOptions<TrackerDbContext> options) : base(options)
        {
        }

        public DbSet<Adversary> Adversaries { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Spirit> Spirits { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
