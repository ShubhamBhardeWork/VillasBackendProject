namespace Villas.API.Models.Domain
{
    public class Villa
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Details { get; set; }
        public decimal Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set;} 
    }
}
