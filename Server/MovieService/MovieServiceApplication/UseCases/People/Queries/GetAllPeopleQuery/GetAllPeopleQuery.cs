using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.People.Queries.GetAllPeopleQuery
{
    public record GetAllPeopleQuery : IQuery<ICollection<PersonDto>>;
}
