using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.GenreSpecifications
{
    public class GenreByNameSpecification : Specification<Genre>
    {
        public GenreByNameSpecification(string name) : base(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
        {
        }
    }
}
