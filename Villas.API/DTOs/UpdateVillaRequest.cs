using System.ComponentModel.DataAnnotations;

namespace Villas.API.DTOs
{
    public class UpdateVillaRequest
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(200)]
        public string? Details { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        public decimal Rate { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Sqft { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Occupancy { get; set; }

        public string? ImageUrl { get; set; }
    }
}