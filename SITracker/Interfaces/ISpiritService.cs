using Microsoft.AspNetCore.Mvc;
using SITracker.Models;

namespace SITracker.Interfaces
{
    public interface ISpiritService
    {
        Task<ActionResult<List<Spirit>>> GetAllSpirits();

        Task<ActionResult<Spirit>> GetSpiritById(long id);

        Task<ActionResult<Spirit>> GetSpiritByPathname(string pathname);
    }
}
