using Microsoft.EntityFrameworkCore;
using Villas.API.Models.Domain;

namespace Villas.API.Data
{
    public class VillaDbContext : DbContext
    {
        public VillaDbContext(DbContextOptions<VillaDbContext> options) : base(options)
        {
            
        }

        public DbSet<Villa> Villas { get; set; }

    }
}
