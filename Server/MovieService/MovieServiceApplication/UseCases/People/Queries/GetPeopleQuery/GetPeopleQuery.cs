using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.People.Queries.GetAllPeopleQuery
{
    public record GetPeopleQuery(string? Name, int PageIndex = 1, int PageSize = 10) : IQuery<PaginatedCollection<PersonDto>>;
}
