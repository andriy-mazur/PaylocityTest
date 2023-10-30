using Api.Dtos.Employee;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using AutoMapper;

namespace Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<GetEmployeeDto?> GetAsync(int id)
    {
        var employee = await _employeeRepository.GetAsync(id);

        return _mapper.Map<GetEmployeeDto>(employee);
    }

    public async Task<List<GetEmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();

        return _mapper.Map<List<GetEmployeeDto>>(employees);
    }
}
