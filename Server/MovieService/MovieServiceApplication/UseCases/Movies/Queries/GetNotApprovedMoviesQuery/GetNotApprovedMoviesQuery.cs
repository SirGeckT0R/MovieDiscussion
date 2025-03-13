using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetNotApprovedMoviesQuery
{
    public record GetNotApprovedMoviesQuery : IQuery<ICollection<DetailedMovieDto>>;
}
