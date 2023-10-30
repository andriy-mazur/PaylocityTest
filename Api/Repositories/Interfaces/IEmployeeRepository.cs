using Api.Models;

namespace Api.Repositories.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetAsync(int id);

    Task<List<Employee>> GetAllAsync();
}
