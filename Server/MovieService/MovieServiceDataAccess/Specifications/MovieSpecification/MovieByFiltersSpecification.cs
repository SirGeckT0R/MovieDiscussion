using MovieServiceDataAccess.Filters;
using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.MovieSpecification
{
    public class MovieByFiltersSpecification : Specification<Movie>
    {
        public MovieByFiltersSpecification(MovieFilters filters) 
            : base(
                  (movie) => 
                  (string.IsNullOrWhiteSpace(filters.Name) || movie.Title.ToLower().Contains(filters.Name.ToLower()))
                  && (filters.Genres == null || !filters.Genres.Except(movie.Genres).Any())
                  && (filters.CrewMember == null || movie.CrewMembers.Select(m => m.PersonId).Contains(filters.CrewMember.Value))
                  ) 
        { }
    }
}
