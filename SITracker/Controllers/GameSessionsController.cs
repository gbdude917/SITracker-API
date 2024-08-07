﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SITracker.Dtos;
using SITracker.Interfaces;
using SITracker.Models;
using System.Security.Claims;

namespace SITracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/game-sessions")]
    public class GameSessionsController : ControllerBase
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly IConfiguration _configuration;

        public GameSessionsController(IGameSessionService service, IConfiguration configuration)
        {
            _gameSessionService = service;
            _configuration = configuration;
        }

        [HttpGet("my-game-sessions")]
        public async Task<ActionResult<List<GameSession>>> GetMyGameSessions()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var gameSessions = await _gameSessionService.GetGameSessionsByUserId(long.Parse(userId));
            return Ok(gameSessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameSession>> GetGameSessionById(long id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var gameSessionActionResult = await _gameSessionService.GetGameSessionById(id);
            var gameSession = gameSessionActionResult.Value;

            // Ensure that the user logged in owns the game session found
            if (gameSession == null)
            {
                return NotFound();
            }

            if (long.Parse(userId) != gameSession.UserId)
            {
                return Forbid();
            }

            return Ok(gameSession);
        }

        [HttpPost]
        public async Task<ActionResult<GameSession>> CreateGameSession([FromBody] GameSessionDto createGameSessionDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure that user that is logged in (jwt) matches the user attached to game session
            if (userId == null || long.Parse(userId) != createGameSessionDto.UserId)
            {
                return Unauthorized();
            }

            try
            {
                var createdGameSessionResult = await _gameSessionService.CreateGameSession(createGameSessionDto);

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
        public async Task<ActionResult<GameSession>> UpdateGameSession(long id, [FromBody] GameSessionDto newGameSession)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure that user that is logged in (jwt) matches the user attached to game session
            if (userId == null || long.Parse(userId) != newGameSession.UserId)
            {
                return Unauthorized();
            }

            try
            {
                var updatedGameSession = await _gameSessionService.UpdateGameSession(id, newGameSession);

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Ensure that user is loggedin
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                var gameSessionActionResult = await _gameSessionService.GetGameSessionById(id);
                var gameSession = gameSessionActionResult.Value;

                // Ensure that the user logged in owns the game session found
                if (gameSession == null)
                {
                    return NotFound();
                }

                if (long.Parse(userId) != gameSession.UserId)
                {
                    return Forbid();
                }

                var result = await _gameSessionService.DeleteGameSession(id);

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
