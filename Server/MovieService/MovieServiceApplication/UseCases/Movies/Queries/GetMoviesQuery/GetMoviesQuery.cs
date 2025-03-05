using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDataAccess.Filters;

namespace MovieServiceApplication.UseCases.Movies.Queries.GetAllMoviesQuery
{
    public record GetMoviesQuery(MovieFilters Filters, int PageIndex = 1, int PageSize = 10) : IQuery<PaginatedCollection<MovieDto>>;
}
