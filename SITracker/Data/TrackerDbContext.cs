﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameSession>()
                .Property(e => e.PlayedOn)
                .HasConversion(new ValueConverter<DateTime, DateTime>(
                    v => v.ToUniversalTime(),    // Convert DateTime to UTC before saving
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));  // Ensure DateTime kind is UTC when reading from database
        }
    }
}
