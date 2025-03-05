using MovieServiceApplication.Dto;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.Interfaces.UseCases.Helpers
{
    public interface IDetailedMovieHelper
    {
        Task<ICollection<Person>> GetDetailedCrewAsync(Movie movie, CancellationToken cancellationToken);
        Task<ICollection<Person>> GetDetailedCrewAsync(ICollection<Movie> movies, CancellationToken cancellationToken);
        Task<ICollection<Genre>> GetDetailedGenresAsync(ICollection<Movie> movies, CancellationToken cancellationToken);
        Task<ICollection<Genre>> GetDetailedGenresAsync(Movie movie, CancellationToken cancellationToken);
        void SetDetailedCrew(ICollection<DetailedMovieDto> movieDtos, ICollection<Person> people);
        void SetDetailedCrew(DetailedMovieDto movieDto, ICollection<Person> people);
        void SetDetailedGenres(ICollection<DetailedMovieDto> movieDtos, ICollection<Genre> genres);
        void SetDetailedGenres(DetailedMovieDto movieDto, ICollection<Genre> genres);
    }
}
