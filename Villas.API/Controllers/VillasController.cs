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

            return Ok(new ApiResponse<List<VillaResponse>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Villas retrieved successfully.",
                Data = villasResponse,
                TraceId = HttpContext.TraceIdentifier
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse<VillaResponse>>> GetVillaById([FromRoute] int id)
        {
            if(id < 1)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Please enter a valid id greater than 0.",
                    Errors = new List<string> { "Id must be greater than 0." },
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            
            var existingVilla = await _villaRepository.GetByIdAsync(id);
            
            if(existingVilla is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Villa with id {id} not found.",
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            var villaResponse = _mapper.Map<VillaResponse>(existingVilla);

            return Ok(new ApiResponse<VillaResponse>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Villa retrieved successfully.",
                Data = villaResponse,
                TraceId = HttpContext.TraceIdentifier
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<VillaResponse>>> CreateVilla([FromBody] CreateVillaRequest createVillaRequest)
        {
            var validationResult = await _createValidator.ValidateAsync(createVillaRequest);

            if (!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                //return BadRequest(validationResult.Errors);
                //return BadRequest(errorMessages);
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Validation failed.",
                    Errors = errorMessages,
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            createVillaRequest.Name = createVillaRequest.Name.Trim();

            var villaName = createVillaRequest.Name;

            var isVillaNameExists = await _villaRepository.IsVillaNameExistsAsync(villaName);

            if(isVillaNameExists)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Villa name already exists.",
                    Errors = new List<string> { "Duplicate villa name is not allowed." },
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            
            var villa = _mapper.Map<Villa>(createVillaRequest);
            
            var createdVilla = await _villaRepository.CreateAsync(villa);
            
            var villaResponse = _mapper.Map<VillaResponse>(createdVilla);

            return CreatedAtAction(nameof(GetVillaById), new { id = villaResponse.Id }, new ApiResponse<VillaResponse>
            {
                Success = true,
                StatusCode = StatusCodes.Status201Created,
                Message = "Villa created successfully.",
                Data = villaResponse,
                TraceId = HttpContext.TraceIdentifier
            });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse<VillaResponse>>> UpdateVilla([FromRoute] int id, [FromBody] UpdateVillaRequest updateVillaRequest)
        {
            if(id < 1)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Please enter a valid id greater than 0.",
                    Errors = new List<string> { "Id must be greater than 0." },
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            
            var validationResult = await _updateValidator.ValidateAsync(updateVillaRequest);
            
            if(!validationResult.IsValid)
            {
                var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                //return BadRequest(validationResult.Errors);
                //return BadRequest(errorMessages);
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Validation failed.",
                    Errors = errorMessages,
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            updateVillaRequest.Name = updateVillaRequest.Name.Trim();

            var villaName = updateVillaRequest.Name;

            var isVillaNameExists = await _villaRepository.IsVillaNameExistsAsync(villaName, id);

            if (isVillaNameExists)
            {
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status409Conflict,
                    Message = "Villa name already exists.",
                    Errors = new List<string> { "Duplicate villa name is not allowed." },
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            var villa = _mapper.Map<Villa>(updateVillaRequest);
            
            var updatedVilla = await _villaRepository.UpdateAsync(id, villa);

            if(updatedVilla is null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Villa with id {id} not found.",
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            
            
            var villaResponse = _mapper.Map<VillaResponse>(updatedVilla);

            return Ok(new ApiResponse<VillaResponse>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Villa updated successfully.",
                Data = villaResponse,
                TraceId = HttpContext.TraceIdentifier
            });
        }
        

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVilla([FromRoute] int id)
        {
            if(id < 1)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Please enter a valid id greater than 0.",
                    Errors = new List<string> { "Id must be greater than 0." },
                    TraceId = HttpContext.TraceIdentifier
                });
            }
            
            var isVillaDeleted = await _villaRepository.DeleteAsync(id);
            
            if(!isVillaDeleted)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Villa with Id {id} not found.",
                    TraceId = HttpContext.TraceIdentifier
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Message = "Villa deleted successfully.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

    }

}
