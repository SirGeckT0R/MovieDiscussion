using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Genres.Queries.GetAllGenresQuery
{
    public record GetAllGenresQuery : IQuery<ICollection<GenreDto>>;
}
