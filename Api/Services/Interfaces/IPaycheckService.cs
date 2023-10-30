using Api.Dtos.Employee;

namespace Api.Services.Interfaces;

public interface IPaycheckService
{
    Task<GetPaycheckDto?> GetPaycheckAsync(int employeeId, DateTime onDate);
}
