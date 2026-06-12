namespace Villas.API.DTOs
{
    public class UpdateVillaRequest
    {
        public required string Name { get; set; }
        public string? Details { get; set; }
        public decimal Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string? ImageUrl { get; set; }
    }
}