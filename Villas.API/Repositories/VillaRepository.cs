using Microsoft.EntityFrameworkCore;
using Villas.API.Data;
using Villas.API.Models.Domain;

namespace Villas.API.Repositories
{
    public class VillaRepository : IVillaRepository
    {
        private readonly VillaDbContext _context;
        public VillaRepository(VillaDbContext context)
        {
            _context = context;
        }

        public async Task<Villa> CreateAsync(Villa villa)
        {
            await _context.Villas.AddAsync(villa);
            await _context.SaveChangesAsync();
            return villa;
        }

        public async Task<IEnumerable<Villa>> GetAllAsync()
        {
            return await _context.Villas.AsNoTracking().ToListAsync();
        }

        public async Task<Villa?> GetByIdAsync(int id)
        {
            return await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
