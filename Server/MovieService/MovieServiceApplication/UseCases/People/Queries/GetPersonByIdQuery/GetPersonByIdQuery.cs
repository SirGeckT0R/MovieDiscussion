using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.People.Queries.GetPersonByIdQuery
{
    public record GetPersonByIdQuery(Guid Id) : IQuery<PersonDto>;
}
