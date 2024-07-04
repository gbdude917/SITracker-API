using Microsoft.AspNetCore.Mvc;
using SITracker.Interfaces;
using SITracker.Models;

namespace SITracker.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SpiritsController : ControllerBase
    {
        private readonly ISpiritService _spiritService;

        public SpiritsController(ISpiritService spiritService)
        {
            _spiritService = spiritService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Spirit>>> GetAllSpirits()
        {
            return await _spiritService.GetAllSpirits();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Spirit>> GetSpiritById(long id)
        {
            return await _spiritService.GetSpiritById(id);
        }

        [HttpGet("pathname/{pathname}")]
        public async Task<ActionResult<Spirit>> GetSpiritByPathname(string pathname)
        {
            return await _spiritService.GetSpiritByPathname(pathname);
        }
    }
}
