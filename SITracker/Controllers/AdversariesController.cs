using Microsoft.AspNetCore.Mvc;
using SITracker.Models;
using SITracker.Interfaces;

namespace SITracker.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdversariesController : ControllerBase
    {
        private readonly IAdversaryService _adversaryService;

        public AdversariesController(IAdversaryService adversaryService)
        {
            _adversaryService = adversaryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Adversary>>> GetAllAdversaries()
        {
            return await _adversaryService.GetAllAdversaries();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Adversary>> GetAdversaryById(long id)
        {
            return await _adversaryService.GetAdversaryById(id);
        }

        [HttpGet("pathname/{pathname}")]
        public async Task<ActionResult<Adversary>> GetAdversaryByPathname(string pathname)
        {
            return await _adversaryService.GetAdversaryByPathname(pathname);
        }
    }
}
