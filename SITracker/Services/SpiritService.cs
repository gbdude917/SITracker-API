using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SITracker.Data;
using SITracker.Interfaces;
using SITracker.Models;

namespace SITracker.Services
{
    public class SpiritService : ISpiritService
    {

        private readonly TrackerDbContext _context;

        public SpiritService(TrackerDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<List<Spirit>>> GetAllSpirits()
        {
            return await _context.Spirits.ToListAsync();
        }

        public async Task<ActionResult<Spirit>> GetSpiritById(long id)
        {
            var spirit = await _context.Spirits.FindAsync(id);

            if (spirit == null) return new NotFoundResult();

            return spirit;
        }

        public async Task<ActionResult<Spirit>> GetSpiritByPathname(string pathname)
        {
            var spirit = await _context.Spirits.SingleOrDefaultAsync(s => s.Pathname == pathname);

            if (spirit == null) return new NotFoundResult();

            return spirit;
        }
    }
}
