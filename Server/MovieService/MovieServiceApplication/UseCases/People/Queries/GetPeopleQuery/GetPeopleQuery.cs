using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.People.Queries.GetAllPeopleQuery
{
    public record GetPeopleQuery(string? Name, int? PageIndex, int? PageSize) : IQuery<ICollection<PersonDto>>;
}
