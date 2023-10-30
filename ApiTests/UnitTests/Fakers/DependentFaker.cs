using Api.Dtos.Dependent;
using Api.Models;
using Bogus;

namespace ApiTests.UnitTests.Fakers;

public sealed class GetDependentDtoFaker : Faker<GetDependentDto>
{
    public GetDependentDtoFaker()
    {
        RuleFor(o => o.Id, f => f.Random.Int(1));
        RuleFor(o => o.Relationship, Relationship.Child);
        RuleFor(o => o.FirstName, f => f.Name.FirstName());
        RuleFor(o => o.LastName, f => f.Name.LastName());
        RuleFor(o => o.DateOfBirth, f => f.Date.Past(f.Random.Int(0, 40)));
    }
}
