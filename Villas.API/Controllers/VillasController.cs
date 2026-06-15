using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Villas.API.DTOs;
using Villas.API.Models.Domain;
using Villas.API.Repositories;

namespace Villas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillasController : ControllerBase
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IValidator<CreateVillaRequest> _createValidator;
        private readonly IValidator<UpdateVillaRequest> _updateValidator;

        public VillasController(IVillaRepository villaRepository, IValidator<CreateVillaRequest> createValidator, IValidator<UpdateVillaRequest> updateValidator)
        {
            _villaRepository = villaRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }


        [HttpGet]
        public async Task<IActionResult> GetVillas()
        {
            try
            {
                var villas = await _villaRepository.GetAllAsync();

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
                }).ToList();

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

                var existingVilla = await _villaRepository.GetByIdAsync(id);

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

        [HttpPost]
        public async Task<ActionResult<VillaResponse>> CreateVilla([FromBody] CreateVillaRequest createVillaRequest)
        {
            try
            {
                if(createVillaRequest is null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Villa is required."
                    });
                }

                var validationResult = await _createValidator.ValidateAsync(createVillaRequest);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var villa = new Villa
                {
                    Name = createVillaRequest.Name,
                    Details = createVillaRequest.Details,
                    Rate = createVillaRequest.Rate,
                    Sqft = createVillaRequest.Sqft,
                    Occupancy = createVillaRequest.Occupancy,
                    ImageUrl = createVillaRequest.ImageUrl
                };

                var createdVilla = await _villaRepository.CreateAsync(villa);

                var villaResponse = new VillaResponse
                {
                    Id = createdVilla.Id,
                    Name = createdVilla.Name,
                    Details = createdVilla.Details,
                    Rate = createdVilla.Rate,
                    Sqft = createdVilla.Sqft,
                    Occupancy = createdVilla.Occupancy,
                    ImageUrl = createdVilla.ImageUrl,
                    CreatedAt = createdVilla.CreatedAt,
                    LastUpdatedAt = createdVilla.LastUpdatedAt
                };

                return CreatedAtAction(nameof(GetVillaById), new { id = villaResponse.Id }, villaResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    //Message = "Something went wrong.",
                    Message = "Error occurred while creating the villa."
                });
                
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<VillaResponse>> UpdateVilla([FromRoute] int id, [FromBody] UpdateVillaRequest updateVillaRequest)
        {
            try
            {
                // validate request like id should be greater than 0 and villa should not null
                // map dto to domain model
                // call repository & they return updated villa or they should return null
                // if null return NotFound()
                // map domain model to dto
                // else send updatedVilla

                if(id < 1)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "please enter a valid id greater than 0."
                    });
                }

                if(updateVillaRequest is null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Villa is required."
                    });
                }

                var validationResult = await _updateValidator.ValidateAsync(updateVillaRequest);

                if(!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var villa = new Villa
                {
                    Name = updateVillaRequest.Name,
                    Details = updateVillaRequest.Details,
                    Rate = updateVillaRequest.Rate,
                    Sqft = updateVillaRequest.Sqft,
                    Occupancy = updateVillaRequest.Occupancy,
                    ImageUrl = updateVillaRequest.ImageUrl
                };

                var updatedVilla = await _villaRepository.UpdateAsync(id, villa);

                if(updatedVilla is null)
                {
                    return NotFound(new 
                    {
                        Success = false,
                        Message = $"Villa with id {id} not found."
                    });
                }

                var villaResponse = new VillaResponse
                {
                    Id = updatedVilla.Id,
                    Name = updatedVilla.Name,
                    Details = updatedVilla.Details,
                    Rate = updatedVilla.Rate,
                    Sqft = updatedVilla.Sqft,
                    Occupancy = updatedVilla.Occupancy,
                    ImageUrl = updatedVilla.ImageUrl,
                    CreatedAt = updatedVilla.CreatedAt,
                    LastUpdatedAt = updatedVilla.LastUpdatedAt
                };

                return Ok(villaResponse);
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Error occurred while updating the villa."
                });
                
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVilla([FromRoute] int id)
        {
            try
            {
                if(id < 1)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "please enter a valid id greater than 0."
                    });
                }

                var isVillaDeleted = await _villaRepository.DeleteAsync(id);

                if(!isVillaDeleted)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = $"Villa with Id {id} not found."
                    });
                }

                return NoContent();
            }

            catch (Exception ex)
            {
                return StatusCode( StatusCodes.Status500InternalServerError,  new 
                { 
                    Success = false,
                    Message = "Error occurred while deleting the villa."
                });
            }
        }

    }
}
