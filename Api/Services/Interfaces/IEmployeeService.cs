using Api.Dtos.Employee;

namespace Api.Services.Interfaces;

public interface IEmployeeService
{
    Task<GetEmployeeDto?> GetAsync(int id);

    Task<List<GetEmployeeDto>> GetAllAsync();
}
