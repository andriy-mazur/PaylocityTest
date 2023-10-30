using Api.Dal;
using Api.Models;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class DependentRepository : IDependentRepository
{
    private readonly PaylocityContext _dbContext;

    public DependentRepository(PaylocityContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Dependent?> GetAsync(int id)
    {
        return await _dbContext.Dependents.Include(e => e.Employee).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Dependent>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _dbContext.Dependents.Where(e => e.EmployeeId == employeeId).ToListAsync();
    }

    public async Task<List<Dependent>> GetAllAsync()
    {
        return await _dbContext.Dependents.ToListAsync();
    }
}
