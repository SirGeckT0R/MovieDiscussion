using MovieServiceApplication.Dto;
using MovieServiceApplication.Interfaces.UseCases.Helpers;
using MovieServiceDataAccess.Interfaces.UnitOfWork;
using MovieServiceDomain.Models;

namespace MovieServiceApplication.UseCases.Movies.Helpers
{
    public class DetailedMovieHelper(IUnitOfWork unitOfWork) : IDetailedMovieHelper
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ICollection<Person>> GetDetailedCrewAsync(Movie movie, CancellationToken cancellationToken)
        {
            var crewMemberIds = movie.CrewMembers.Select(x => x.PersonId);

            var people = await _unitOfWork.People.GetFromListOfIdsAsync(crewMemberIds, cancellationToken);

            return people;
        }

        public async Task<ICollection<Person>> GetDetailedCrewAsync(ICollection<Movie> movies, CancellationToken cancellationToken)
        {
            var crewMemberIds = movies.SelectMany(movie => movie.CrewMembers.Select(x => x.PersonId))
                                      .Distinct();

            var people = await _unitOfWork.People.GetFromListOfIdsAsync(crewMemberIds, cancellationToken);

            return people;
        }

        public async Task<ICollection<Genre>> GetDetailedGenresAsync(Movie movie, CancellationToken cancellationToken)
        {
            var genres = await _unitOfWork.Genres.GetFromListOfIdsAsync(movie.Genres, cancellationToken);

            return genres;
        }

        public async Task<ICollection<Genre>> GetDetailedGenresAsync(ICollection<Movie> movies, CancellationToken cancellationToken)
        {
            var genresFromMovies = movies.SelectMany(movie => movie.Genres)
                                         .Distinct();

            var genres = await _unitOfWork.Genres.GetFromListOfIdsAsync(genresFromMovies, cancellationToken);

            return genres;
        }

        public void SetDetailedCrew(ICollection<DetailedMovieDto> movieDtos, ICollection<Person> people)
        {
            foreach (var movieDto in movieDtos)
            {
                SetDetailedCrew(movieDto, people);
            }
        }

        public void SetDetailedCrew(DetailedMovieDto movieDto, ICollection<Person> people)
        {
            var crewList = movieDto.CrewMembers
                                    .Join(
                                        people,
                                        crewMember => crewMember.PersonId,
                                        person => person.Id,
                                        (crewMember, person) => new CrewMemberDto
                                        {
                                            PersonId = crewMember.PersonId,
                                            Role = crewMember.Role,
                                            FullName = $"{person.FirstName} {person.LastName}"
                                        }
                                    )
                                    .ToList();

            movieDto.CrewMembers = crewList;
        }

        public void SetDetailedGenres(ICollection<DetailedMovieDto> movieDtos, ICollection<Genre> genres)
        {
            foreach (var movieDto in movieDtos)
            {
                SetDetailedGenres(movieDto, genres);
            }
        }

        public void SetDetailedGenres(DetailedMovieDto movieDto, ICollection<Genre> genres)
        {
            var genreList = movieDto.Genres
                                     .Join(
                                          genres,
                                          genreDto => genreDto.Id,
                                          genre => genre.Id,
                                          (genreDto, genre) => new GenreDto 
                                          {
                                              Id = genreDto.Id,
                                              Name = genre.Name
                                          }
                                     )
                                     .ToList();

            movieDto.Genres = genreList;
        }
    }
}
