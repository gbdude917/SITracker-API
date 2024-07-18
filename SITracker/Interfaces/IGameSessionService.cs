using Microsoft.AspNetCore.Mvc;
using SITracker.Models;
using SITracker.Dtos;


namespace SITracker.Interfaces
{
    public interface IGameSessionService
    {
        Task<ActionResult<List<GameSession>>> GetGameSessionsByUserId(long id);

        Task<ActionResult<GameSession>> GetGameSessionById(long id);

        Task<ActionResult<GameSession>> CreateGameSession(GameSessionDto gameSession);

        Task<ActionResult<GameSession>> UpdateGameSession(long id, GameSessionDto newGameSession);

        Task<ActionResult> DeleteGameSession(long id);
    }
}
