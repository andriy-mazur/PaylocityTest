using Api.Dtos.Dependent;

namespace Api.Services.Interfaces;

public interface IDependentService
{
    Task<GetDependentDto?> GetAsync(int id);

    Task<List<GetDependentDto>> GetByEmployeeIdAsync(int employeeId);

    Task<List<GetDependentDto>> GetAllAsync();
}
