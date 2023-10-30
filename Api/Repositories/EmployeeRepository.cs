using Api.Dal;
using Api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly PaylocityContext _dbContext;

    public EmployeeRepository(PaylocityContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Employee?> GetAsync(int id)
    {
        return await _dbContext.Employees.Include(i => i.Dependents).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _dbContext.Employees.Include(i => i.Dependents).ToListAsync();
    }
}
