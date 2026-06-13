using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Villas.API.Data;
using Villas.API.DTOs;

namespace Villas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillasController : ControllerBase
    {
        private readonly VillaDbContext _context;
        public VillasController(VillaDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetVillas()
        {
            try
            {
                var villas = await _context.Villas.AsNoTracking().ToListAsync();
                var villasResponse = villas.Select(v => new VillaResponse
                {
                    Id = v.Id,
                    Name = v.Name,
                    Details = v.Details,
                    Rate = v.Rate,
                    Sqft = v.Sqft,
                    Occupancy = v.Occupancy,
                    ImageUrl = v.ImageUrl,
                    CreatedAt = v.CreatedAt,
                    LastUpdatedAt = v.LastUpdatedAt
                });

                //throw new Exception("Test exception for error handling validation.");

                return Ok(villasResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {   
                        Status = false,
                        Message = "Something went wrong.",
                    }
                );
            }
        }
    }
}
