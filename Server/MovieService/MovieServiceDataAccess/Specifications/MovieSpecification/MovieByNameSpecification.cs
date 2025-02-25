using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.MovieSpecification
{
    public class MovieByNameSpecification : Specification<Movie>
    {
        public MovieByNameSpecification(string? name) : base((movie) => string.IsNullOrWhiteSpace(name) || 
                                                                        movie.Title.ToLower().Contains(name.ToLower())) { }
    }
}
