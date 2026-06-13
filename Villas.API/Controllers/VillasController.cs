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
                var villasResponse = await _context.Villas
                    .AsNoTracking()
                    .Select(v => new VillaResponse
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
                    })
                    .ToListAsync();

                //throw new Exception("Test exception for error handling validation.");

                return Ok(villasResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {   
                        Success = false,
                        Message = "Something went wrong.",
                    }
                );
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetVillaById([FromRoute] int id)
        {
            try
            {
                if(id < 1)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Please enter a valid id greater than 0."
                    });
                }

                var existingVilla = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

                if(existingVilla is null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Villa with Id {id} not found."
                    });
                }

                var villaResponse = new VillaResponse
                {
                    Id = existingVilla.Id,
                    Name = existingVilla.Name,
                    Details = existingVilla.Details,
                    Rate = existingVilla.Rate,
                    Sqft = existingVilla.Sqft,
                    Occupancy = existingVilla.Occupancy,
                    ImageUrl = existingVilla.ImageUrl,
                    CreatedAt = existingVilla.CreatedAt,
                    LastUpdatedAt = existingVilla.LastUpdatedAt
                };

                return Ok(villaResponse);

            }
            catch (Exception ex)
            {

                return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    new
                    {
                        Success = false,
                        Message = "Something went wrong."
                    }
                );

            }
        }
    }
}
