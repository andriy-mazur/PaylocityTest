using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using AutoMapper;

namespace Api.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Models.Employee, GetEmployeeDto>();
        CreateMap<Models.Dependent, GetDependentDto>();
    }
}
