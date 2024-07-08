using Microsoft.AspNetCore.Mvc;
using SITracker.Models;
using SITracker.DTOs;


namespace SITracker.Interfaces
{
    public interface IGameSessionService
    {
        Task<ActionResult<List<GameSession>>> GetGameSessionsByUserId(long id);

        Task<ActionResult<GameSession>> GetGameSessionById(long id);

        Task<ActionResult<GameSession>> CreateGameSession(CreateGameSessionDto gameSession);

        Task<ActionResult<GameSession>> UpdateGameSession(long id,  GameSession newGameSession);

        Task<ActionResult> DeleteGameSession(long id);
    }
}
