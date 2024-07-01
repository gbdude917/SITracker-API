using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SITracker.Data;
using SITracker.Interfaces;
using SITracker.Models;

namespace SITracker.Services
{
    public class AdversaryService : IAdversaryService
    {

        private readonly TrackerDbContext _context;

        public AdversaryService(TrackerDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<List<Adversary>>> GetAllAdversaries()
        {
            return await _context.Adversaries.ToListAsync();
        }

        public async Task<ActionResult<Adversary>> GetAdversaryById(long id)
        {
            var adversary = await _context.Adversaries.FindAsync(id);

            if (adversary == null) return new NotFoundResult();

            return adversary;
        }

        public async Task<ActionResult<Adversary>> GetAdversaryByPathname(string pathname)
        {
            var adversary = await _context.Adversaries.SingleOrDefaultAsync(a => a.Pathname == pathname);

            if (adversary == null) return new NotFoundResult();

            return adversary;   
        }
    }
}
