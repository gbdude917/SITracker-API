using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SITracker.Data;
using SITracker.DTOs;
using SITracker.Interfaces;
using SITracker.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;


namespace SITracker.Services
{
    public class GameSessionService : IGameSessionService
    {
        private readonly TrackerDbContext _context;

        public GameSessionService(TrackerDbContext context) 
        {
            _context = context;
        }

        public async Task<ActionResult<List<GameSession>>> GetGameSessionsByUserId(long id)
        {
            var gameSessions = await _context.GameSessions.Where(gs => gs.UserId == id).ToListAsync();

            return gameSessions;
        }

        public async Task<ActionResult<GameSession>> GetGameSessionById(long id)
        {
            var gameSession = await _context.GameSessions
                .Include(gs => gs.User)
                .Include(gs => gs.Spirit)
                .Include(gs => gs.Adversary)
                .FirstOrDefaultAsync(gs => gs.Id == id);

            if (gameSession == null) return new NotFoundResult();

            return gameSession;
        }

        public async Task<ActionResult<GameSession>> CreateGameSession(CreateGameSessionDto createGameSessionDto)
        {
            if (createGameSessionDto.PlayedOn == null) createGameSessionDto.PlayedOn = DateTime.UtcNow;
            
            // Retrieve the User, Spirit, and Adversary objects
            var user = await _context.Users.FindAsync(createGameSessionDto.UserId);
            var spirit = await _context.Spirits.FindAsync(createGameSessionDto.SpiritId);
            var adversary = await _context.Adversaries.FindAsync(createGameSessionDto.AdversaryId);

            if (user == null) throw new ArgumentException("Invalid user.");
            if (spirit == null) throw new ArgumentException("Invalid spirit.");
            if (adversary == null) throw new ArgumentException("Invalid adversary.");

            var newGameSession = new GameSession
            {
                UserId = createGameSessionDto.UserId,
                User = user,
                SpiritId = createGameSessionDto.SpiritId,
                Spirit = spirit,
                AdversaryId = createGameSessionDto.AdversaryId,
                Adversary = adversary,
                Board = createGameSessionDto.Board,
                SessionName = createGameSessionDto.SessionName,
                Description = createGameSessionDto.Description,
                PlayedOn = createGameSessionDto.PlayedOn,
                Result = createGameSessionDto.Result,
                IsCompleted = createGameSessionDto.IsCompleted
            };

            _context.GameSessions.Add(newGameSession);
            await _context.SaveChangesAsync(); 

            return new CreatedAtActionResult("CreateGameSession", "GameSessions", new { id =  newGameSession.Id }, newGameSession);
        }

        public async Task<ActionResult<GameSession>> UpdateGameSession(long id, GameSession newGameSession)
        {
            var oldGameSession = await _context.GameSessions.FindAsync(id);

            if (oldGameSession == null) return new NotFoundResult();

            oldGameSession.User = newGameSession.User;
            oldGameSession.Spirit = newGameSession.Spirit;
            oldGameSession.Adversary = newGameSession.Adversary;
            oldGameSession.Board = newGameSession.Board;
            oldGameSession.SessionName = newGameSession.SessionName;
            oldGameSession.Description = newGameSession.Description;
            oldGameSession.PlayedOn = newGameSession.PlayedOn;
            oldGameSession.Result = newGameSession.Result;
            oldGameSession.IsCompleted = newGameSession.IsCompleted;

            _context.GameSessions.Update(oldGameSession);
            await _context.SaveChangesAsync();

            return oldGameSession;
        }

        public async Task<ActionResult> DeleteGameSession(long id)
        {
            var gameSession = await _context.GameSessions.FindAsync(id);

            if (gameSession == null) return new NotFoundResult();

            _context.GameSessions.Remove(gameSession);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
