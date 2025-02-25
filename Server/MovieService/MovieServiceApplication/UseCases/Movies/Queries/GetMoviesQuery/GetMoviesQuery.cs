using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetAllMoviesQuery
{
    public record GetMoviesQuery(string? Name, int PageIndex = 1, int PageSize = 10) : IQuery<PaginatedCollection<MovieDto>>;
}
