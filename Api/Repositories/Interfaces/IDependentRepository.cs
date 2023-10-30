using Api.Models;

namespace Api.Repositories.Interfaces;

public interface IDependentRepository
{
    Task<Dependent?> GetAsync(int id);

    Task<List<Dependent>> GetByEmployeeIdAsync(int employeeId);

    Task<List<Dependent>> GetAllAsync();
}
