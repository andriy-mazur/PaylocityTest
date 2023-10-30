using Api.Dtos.Dependent;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;

    public DependentsController(IDependentService dependentService)
    {
        _dependentService = dependentService ?? throw new ArgumentNullException(nameof(dependentService));
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        try
        {
            var dependent = await _dependentService.GetAsync(id);

            if (dependent == null)
            {
                return NotFound(id);
            }

            return new ApiResponse<GetDependentDto>
            {
                Data = dependent
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<GetDependentDto>
            {
                Data = null,
                Error = ex.Message,
                Message = "An error occurred retrieving Dependent",
                Success = false
            };
        }
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        try
        {
            var dependents = await _dependentService.GetAllAsync();

            return new ApiResponse<List<GetDependentDto>>
            {
                Data = dependents
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<GetDependentDto>>
            {
                Data = null,
                Error = ex.Message,
                Message = "An error occurred retrieving Dependents",
                Success = false
            };
        }
    }
}

