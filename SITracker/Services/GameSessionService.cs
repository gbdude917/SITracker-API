using Microsoft.AspNetCore.Mvc;
using SITracker.Data;
using SITracker.Dtos;
using SITracker.Interfaces;
using SITracker.Models;
using Microsoft.EntityFrameworkCore;

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
            var gameSessions = await _context.GameSessions.Where(gs => gs.UserId == id)
                .Include(gs => gs.User)
                .Include(gs => gs.Spirit)
                .Include(gs => gs.Adversary)
                .ToListAsync();

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

        public async Task<ActionResult<GameSession>> CreateGameSession(GameSessionDto gameSessionDto)
        {
            if (gameSessionDto.PlayedOn == null) gameSessionDto.PlayedOn = DateTime.UtcNow;
            
            // Retrieve the User, Spirit, and Adversary objects
            var user = await _context.Users.FindAsync(gameSessionDto.UserId);
            var spirit = await _context.Spirits.FindAsync(gameSessionDto.SpiritId);
            var adversary = await _context.Adversaries.FindAsync(gameSessionDto.AdversaryId);

            if (user == null) throw new ArgumentException("Invalid user.");
            if (spirit == null) throw new ArgumentException("Invalid spirit.");
            if (adversary == null) throw new ArgumentException("Invalid adversary.");

            var newGameSession = new GameSession
            {
                UserId = gameSessionDto.UserId,
                User = user,
                SpiritId = gameSessionDto.SpiritId,
                Spirit = spirit,
                AdversaryId = gameSessionDto.AdversaryId,
                Adversary = adversary,
                Board = gameSessionDto.Board,
                SessionName = gameSessionDto.SessionName,
                Description = gameSessionDto.Description,
                PlayedOn = gameSessionDto.PlayedOn,
                Result = gameSessionDto.Result,
                IsCompleted = gameSessionDto.IsCompleted
            };

            _context.GameSessions.Add(newGameSession);
            await _context.SaveChangesAsync(); 

            return new CreatedAtActionResult("CreateGameSession", "GameSessions", new { id =  newGameSession.Id }, newGameSession);
        }

        public async Task<ActionResult<GameSession>> UpdateGameSession(long id, GameSessionDto newGameSession)
        {
            var oldGameSession = await _context.GameSessions.FindAsync(id);

            if (oldGameSession == null) return new NotFoundResult();

            // Get the user, spirit, and adversary by id
            var user = await _context.Users.FindAsync(newGameSession.UserId);
            var spirit = await _context.Spirits.FindAsync(newGameSession.SpiritId);
            var adversary = await _context.Adversaries.FindAsync(newGameSession.AdversaryId);

            if (user == null) throw new ArgumentException("Invalid user.");
            if (spirit == null) throw new ArgumentException("Invalid spirit.");
            if (adversary == null) throw new ArgumentException("Invalid adversary.");

            oldGameSession.User = user;
            oldGameSession.Spirit = spirit;
            oldGameSession.Adversary = adversary;
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
