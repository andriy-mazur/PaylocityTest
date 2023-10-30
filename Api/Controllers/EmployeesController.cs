using Api.Dtos.Employee;
using Api.Models;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IPaycheckService _paycheckService;

    public EmployeesController(
        IEmployeeService employeeService,
        IPaycheckService paycheckService)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
        _paycheckService = paycheckService ?? throw new ArgumentNullException(nameof(paycheckService));
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        try
        {
            var employee = await _employeeService.GetAsync(id);

            if (employee == null)
            {
                return NotFound(id);
            }

            return new ApiResponse<GetEmployeeDto>
            {
                Data = employee
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<GetEmployeeDto>
            {
                Data = null,
                Error = ex.Message,
                Message = "An error occurred retrieving Employee",
                Success = false
            };
        }
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        try
        {
            var employees = await _employeeService.GetAllAsync();

            return new ApiResponse<List<GetEmployeeDto>>
            {
                Data = employees
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<GetEmployeeDto>>
            {
                Data = null,
                Error = ex.Message,
                Message = "An error occurred retrieving Employees",
                Success = false
            };
        }
    }

    [SwaggerOperation(Summary = "Get Employee's paycheck")]
    [HttpGet("{employeeId:int}/paycheck")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> GetEmployeePaycheck(int employeeId)
    {
        try
        {
            var paycheck = await _paycheckService.GetPaycheckAsync(employeeId, DateTime.Today);

            return Ok(new ApiResponse<GetPaycheckDto>
            {
                Data = paycheck
            });

        }
        catch (Exception ex)
        {
            return new ApiResponse<GetPaycheckDto>
            {
                Data = null,
                Error = ex.Message,
                Message = "An error occurred calculating Employee Paycheck",
                Success = false
            };
        }
    }
}
