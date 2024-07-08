using Microsoft.AspNetCore.Mvc;
using SITracker.DTOs;
using SITracker.Interfaces;
using SITracker.Models;
using System.Security.Claims;

namespace SITracker.Controllers
{
    [ApiController]
    [Route("api/v1/game-sessions")]
    public class GameSessionsController : ControllerBase
    {
        private readonly IGameSessionService _service;

        public GameSessionsController(IGameSessionService service)
        {
            _service = service;
        }

        [HttpGet("my-game-sessions")]
        public async Task<ActionResult<List<GameSession>>> GetMyGameSessions()
        {
            // TODO: Still need to test this with JWT/cookies
            // Get the user ID from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId == null)
            {
                return Unauthorized();
            }

            var gameSessions = await _service.GetGameSessionsByUserId(long.Parse(userId));

            return Ok(gameSessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameSession>> GetGameSessionById(long id)
        {
            return await _service.GetGameSessionById(id);
        }

        [HttpPost]
        public async Task<ActionResult<GameSession>> CreateGameSession([FromBody] CreateGameSessionDto createGameSessionDto)
        {
            try
            {
                var createdGameSessionResult = await _service.CreateGameSession(createGameSessionDto);

                if (createdGameSessionResult.Result is ObjectResult objectResult && objectResult.Value is GameSession createdGameSession)
                {
                    return CreatedAtAction(nameof(GetGameSessionById), new { id = createdGameSession.Id }, createdGameSession);
                }
                else
                {
                    return StatusCode(500, "Failed to create game session");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, "Failed to create game session: " + e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GameSession>> UpdateGameSession(long id, [FromBody] GameSession newGameSession)
        {
            try
            {
                var updatedGameSession = await _service.UpdateGameSession(id, newGameSession);

                if (updatedGameSession == null) return NotFound("Game Session not found!");

                return Ok(updatedGameSession);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Failed to update game session: " + e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGameSession(long id)
        {
            try
            {
                var result = await _service.DeleteGameSession(id);

                if (result == null) return NotFound("Game session not found!");

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, "Failed to delete game session: " + e.Message);
            }
        }
    }
}
