using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetAllMoviesQuery
{
    public record GetAllMoviesQuery : IQuery<ICollection<MovieDto>>;
}
