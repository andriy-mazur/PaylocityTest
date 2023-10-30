using System.Collections.Generic;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Bogus;

namespace ApiTests.UnitTests.Fakers;

public sealed class GetEmployeeDtoFaker : Faker<GetEmployeeDto>
{
    public GetEmployeeDtoFaker()
    {
        RuleFor(o => o.Id, f => f.Random.Int(1));
        RuleFor(o => o.FirstName, f => f.Name.FirstName());
        RuleFor(o => o.LastName, f => f.Name.LastName());
        RuleFor(o => o.Salary, f => f.Random.Int(50, 200) * 1000);
        RuleFor(o => o.DateOfBirth, f => f.Date.Past(f.Random.Int(20, 60)));
        RuleFor(o => o.Dependents, new List<GetDependentDto>());
    }
}
