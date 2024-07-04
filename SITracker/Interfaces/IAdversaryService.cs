using Microsoft.AspNetCore.Mvc;
using SITracker.Models;

namespace SITracker.Interfaces
{
    public interface IAdversaryService
    {
        Task<ActionResult<List<Adversary>>> GetAllAdversaries();

        Task<ActionResult<Adversary>> GetAdversaryById(long id);

        Task<ActionResult<Adversary>> GetAdversaryByPathname(string pathname);
    }
}
