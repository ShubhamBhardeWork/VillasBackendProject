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
        public async Task<ActionResult<ApiResponse<List<VillaResponse>>>> GetVillas()
        {
            var villas = await _villaRepository.GetAllAsync();
            
            var villasResponse = _mapper.Map<List<VillaResponse>>(villas);

            //throw new Exception("Test exception for error handling validation.");

            return Ok(ApiResponse<List<VillaResponse>>.Ok(
                villasResponse, 
                "Villas retrieved successfully.", 
                HttpContext.TraceIdentifier
            ));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<VillaResponse>>> GetVillaById([FromRoute] int id)
        {
            if(id < 1)
            {
                return BadRequest(ApiResponse<object>.BadRequest(
                    "Please enter a valid id greater than 0.",
                    HttpContext.TraceIdentifier,
                    new List<string> { "Id must be greater than 0." }
                ));
            }
            
            var existingVilla = await _villaRepository.GetByIdAsync(id);
            
            if(existingVilla is null)
            {
                return NotFound(ApiResponse<object>.NotFound(
                    $"Villa with id {id} not found.",
                    HttpContext.TraceIdentifier
                ));
            }

            var villaResponse = _mapper.Map<VillaResponse>(existingVilla);

            return Ok(ApiResponse<VillaResponse>.Ok(
                villaResponse,
                "Villa retrieved successfully.",
                HttpContext.TraceIdentifier
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<VillaResponse>>> CreateVilla([FromBody] CreateVillaRequest createVillaRequest)
        {
            var validationResult = await _createValidator.ValidateAsync(createVillaRequest);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                return BadRequest(ApiResponse<object>.BadRequest(
                    "Validation failed.",
                    HttpContext.TraceIdentifier,
                    errorMessages
                ));
            }

            createVillaRequest.Name = createVillaRequest.Name.Trim();

            var villaName = createVillaRequest.Name;

            var isVillaNameExists = await _villaRepository.IsVillaNameExistsAsync(villaName);

            if(isVillaNameExists)
            {
                return Conflict(ApiResponse<object>.Conflict(
                    "Villa name already exists.",
                    HttpContext.TraceIdentifier,
                    new List<string> { "Duplicate villa name is not allowed." }
                ));
            }
            
            var villa = _mapper.Map<Villa>(createVillaRequest);
            
            var createdVilla = await _villaRepository.CreateAsync(villa);
            
            var villaResponse = _mapper.Map<VillaResponse>(createdVilla);

            return CreatedAtAction(nameof(GetVillaById), new { id = villaResponse.Id }, ApiResponse<VillaResponse>.Created(
                villaResponse,
                "Villa created successfully.",
                HttpContext.TraceIdentifier
            ));
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<VillaResponse>>> UpdateVilla([FromRoute] int id, [FromBody] UpdateVillaRequest updateVillaRequest)
        {
            if (id < 1)
            {
                return BadRequest(ApiResponse<object>.BadRequest(
                    "Please enter a valid id greater than 0.",
                    HttpContext.TraceIdentifier,
                    new List<string> { "Id must be greater than 0." }
                ));
            }

            var validationResult = await _updateValidator.ValidateAsync(updateVillaRequest);
            
            if(!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                return BadRequest(ApiResponse<object>.BadRequest(
                    "Validation failed.",
                    HttpContext.TraceIdentifier,
                    errorMessages
                ));
            }

            var existingVilla = await _villaRepository.GetByIdForUpdateAsync(id);

            if (existingVilla is null)
            {
                return NotFound(ApiResponse<object>.NotFound(
                    $"Villa with id {id} not found.",
                    HttpContext.TraceIdentifier
                ));
            }

            updateVillaRequest.Name = updateVillaRequest.Name.Trim();

            var villaName = updateVillaRequest.Name;

            var isVillaNameExists = await _villaRepository.IsVillaNameExistsAsync(villaName, id);

            if (isVillaNameExists)
            {
                return Conflict(ApiResponse<object>.Conflict(
                    "Villa name already exists.",
                    HttpContext.TraceIdentifier,
                    new List<string> { "Duplicate villa name is not allowed." }
                ));
            }

            _mapper.Map(updateVillaRequest, existingVilla);

            var updatedVilla = await _villaRepository.UpdateAsync(existingVilla);

            var villaResponse = _mapper.Map<VillaResponse>(updatedVilla);

            return Ok(ApiResponse<VillaResponse>.Ok(
                villaResponse,
                "Villa updated successfully.",
                HttpContext.TraceIdentifier
            ));
        }
        

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVilla([FromRoute] int id)
        {
            if(id < 1)
            {
                return BadRequest(ApiResponse<object>.BadRequest(
                    "Please enter a valid id greater than 0.",
                    HttpContext.TraceIdentifier,
                    new List<string> { "Id must be greater than 0." }
                ));
            }

            var isVillaDeleted = await _villaRepository.DeleteAsync(id);
            
            if(!isVillaDeleted)
            {
                return NotFound(ApiResponse<object>.NotFound(
                    $"Villa with id {id} not found.",
                    HttpContext.TraceIdentifier
                ));
            }

            return Ok(ApiResponse<object>.Ok(
                null, 
                "Villa deleted successfully.", 
                HttpContext.TraceIdentifier
            ));
        }

    }

}
