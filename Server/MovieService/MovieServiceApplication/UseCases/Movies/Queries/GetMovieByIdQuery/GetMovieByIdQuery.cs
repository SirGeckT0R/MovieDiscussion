using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetMovieByIdQuery
{
    public record GetMovieByIdQuery(Guid Id) : IQuery<DetailedMovieDto>;
}
