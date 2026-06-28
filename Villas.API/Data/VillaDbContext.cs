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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var villas = new List<Villa>
            {
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Luxurious villa with stunning ocean views and private beach access.",
                    Rate = 500,
                    Sqft = 2500,
                    Occupancy = 6,
                    ImageUrl = "https://static.vecteezy.com/system/resources/thumbnails/035/974/293/small_2x/ai-generated-luxury-home-exterior-and-pool-on-sunny-day-with-blue-sky-free-photo.jpg",
                    CreatedAt = new DateTime(2024, 1, 1),
                    LastUpdatedAt = null
                },
                new Villa
                {
                    Id = 2,
                    Name = "Diamond Villa",
                    Details = "Elegant villa with marble interiors and panoramic mountain views.",
                    Rate = 750,
                    Sqft = 3200,
                    Occupancy = 8,
                    ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.RpZHEyv0mnqtn6TdVjQNlwHaE7?pid=Api&P=0&h=180",
                    CreatedAt = new DateTime(2018, 6, 1),
                    LastUpdatedAt = new DateTime(2020, 8, 12)
                },
                new Villa
                {
                    Id = 3,
                    Name = "Pool Villa",
                    Details = "Modern villa featuring an infinity pool and outdoor entertainment area.",
                    Rate = 350,
                    Sqft = 1800,
                    Occupancy = 4,
                    ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.kct6TCUyMIKQzWa2uXBNBQHaE8?pid=Api&P=0&h=180",
                    CreatedAt = new DateTime(2025, 2, 22),
                    LastUpdatedAt = null
                },
                new Villa
                {
                    Id = 4,
                    Name = "Luxury Villa",
                    Details = "Premium villa with spa facilities and concierge services.",
                    Rate = 900,
                    Sqft = 4000,
                    Occupancy = 10,
                    ImageUrl = "https://tse1.mm.bing.net/th/id/OIP.Ebeiw1CrSAS7JNkOrthplAHaEu?pid=Api&P=0&h=180",
                    CreatedAt = new DateTime(2024, 2, 14),
                    LastUpdatedAt = new DateTime(2026, 4, 12)
                },
                new Villa
                {
                    Id = 5,
                    Name = "Garden Villa",
                    Details = "Charming villa surrounded by tropical gardens and nature.",
                    Rate = 275,
                    Sqft = 1500,
                    Occupancy = 3,
                    ImageUrl = "https://tse3.mm.bing.net/th/id/OIP.FkHSoEx_TOwMkABTf2IrlQHaE8?rs=1&pid=ImgDetMain&o=7&rm=3",
                    CreatedAt = new DateTime(2024, 3, 1),
                    LastUpdatedAt = new DateTime(2025, 6, 7)
                }
            };

            // data seeding
            modelBuilder.Entity<Villa>().HasData(villas);

            // unique indexing
            modelBuilder.Entity<Villa>().HasIndex(v => v.Name).IsUnique();

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Name).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().Property(u => u.Role).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

        }

    }
}
