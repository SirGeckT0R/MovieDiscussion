using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Genres.Queries.GetGenreByIdQuery
{
    public record GetGenreByIdQuery(Guid Id) : IQuery<GenreDto>;
}
