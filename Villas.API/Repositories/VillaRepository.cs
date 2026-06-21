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

        public async Task<bool> DeleteAsync(int id)
        {
            var existingVilla = await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (existingVilla is null)
            {
                return false;
            }

            _context.Villas.Remove(existingVilla);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Villa>> GetAllAsync()
        {
            return await _context.Villas.AsNoTracking().ToListAsync();
        }

        public async Task<Villa?> GetByIdAsync(int id)
        {
            return await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Villa?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Villas.FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<bool> IsVillaNameExistsAsync(string name, int? currentVillaId = null)
        {
            return await _context.Villas.AnyAsync(v => v.Name == name && v.Id != currentVillaId);
        }

        public async Task<Villa> UpdateAsync(Villa villa)
        {
            villa.LastUpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return villa;
        }
    }
}
