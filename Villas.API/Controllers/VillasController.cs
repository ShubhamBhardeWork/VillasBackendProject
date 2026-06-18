using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Villas.API.DTOs;
using Villas.API.Models.Domain;
using Villas.API.Repositories;
using AutoMapper;

namespace Villas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillasController : ControllerBase
    {
        private readonly IVillaRepository _villaRepository;
        private readonly IValidator<CreateVillaRequest> _createValidator;
        private readonly IValidator<UpdateVillaRequest> _updateValidator;
        private readonly IMapper _mapper;

        public VillasController(IVillaRepository villaRepository, IValidator<CreateVillaRequest> createValidator, IValidator<UpdateVillaRequest> updateValidator, IMapper mapper)
        {
            _villaRepository = villaRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetVillas()
        {
            var villas = await _villaRepository.GetAllAsync();
            
            var villasResponse = _mapper.Map<List<VillaResponse>>(villas);
            
            //throw new Exception("Test exception for error handling validation.");

            return Ok(villasResponse);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetVillaById([FromRoute] int id)
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
            
            var villaResponse = _mapper.Map<VillaResponse>(existingVilla);
            
            return Ok(villaResponse);
        }

        [HttpPost]
        public async Task<ActionResult<VillaResponse>> CreateVilla([FromBody] CreateVillaRequest createVillaRequest)
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

            createVillaRequest.Name = createVillaRequest.Name.Trim();

            var villaName = createVillaRequest.Name;

            var isVillaNameExists = await _villaRepository.IsVillaNameExistsAsync(villaName);

            if(isVillaNameExists)
            {
                return Conflict(new
                {
                    Success = false,
                    Message = "Villa name already exists."
                });
            }
            
            var villa = _mapper.Map<Villa>(createVillaRequest);
            
            var createdVilla = await _villaRepository.CreateAsync(villa);
            
            var villaResponse = _mapper.Map<VillaResponse>(createdVilla);
            
            return CreatedAtAction(nameof(GetVillaById), new { id = villaResponse.Id }, villaResponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<VillaResponse>> UpdateVilla([FromRoute] int id, [FromBody] UpdateVillaRequest updateVillaRequest)
        {
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

            updateVillaRequest.Name = updateVillaRequest.Name.Trim();

            var villaName = updateVillaRequest.Name;

            var isVillaNameExists = await _villaRepository.IsVillaNameExistsAsync(villaName, id);

            if (isVillaNameExists)
            {
                return Conflict(new
                {
                    Success = false,
                    Message = "Villa name already exists."
                });
            }

            var villa = _mapper.Map<Villa>(updateVillaRequest);
            
            var updatedVilla = await _villaRepository.UpdateAsync(id, villa);

            if(updatedVilla is null)
            {
                return NotFound(new 
                {
                    Success = false,
                    Message = $"Villa with id {id} not found."
                });
            }
            
            
            var villaResponse = _mapper.Map<VillaResponse>(updatedVilla);
            
            return Ok(villaResponse);
        }
        

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVilla([FromRoute] int id)
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

    }

}
