using Api.Dtos.Dependent;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using AutoMapper;

namespace Api.Services;

public class DependentService : IDependentService
{
    private readonly IDependentRepository _dependentRepository;
    private readonly IMapper _mapper;

    public DependentService(IDependentRepository dependentRepository, IMapper mapper)
    {
        _dependentRepository = dependentRepository ?? throw new ArgumentNullException(nameof(dependentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<GetDependentDto?> GetAsync(int id)
    {
        var dependent = await _dependentRepository.GetAsync(id);

        return _mapper.Map<GetDependentDto>(dependent);
    }

    public async Task<List<GetDependentDto>> GetByEmployeeIdAsync(int employeeId)
    {
        var dependents = await _dependentRepository.GetByEmployeeIdAsync(employeeId);

        return _mapper.Map<List<GetDependentDto>>(dependents);
    }

    public async Task<List<GetDependentDto>> GetAllAsync()
    {
        var dependents = await _dependentRepository.GetAllAsync();

        return _mapper.Map<List<GetDependentDto>>(dependents);
    }
}
