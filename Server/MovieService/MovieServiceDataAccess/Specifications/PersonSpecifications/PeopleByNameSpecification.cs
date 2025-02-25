using MovieServiceDomain.Models;

namespace MovieServiceDataAccess.Specifications.PersonSpecifications
{
    public class PeopleByNameSpecification : Specification<Person>
    {
        public PeopleByNameSpecification(string? name)
            : 
            base((person) =>
                    string.IsNullOrWhiteSpace(name) || name.Split(' ', StringSplitOptions.None)
                                        .All(
                                            part => person.FirstName.ToLower().Contains(part.ToLower())
                                                || person.LastName.ToLower().Contains(part.ToLower())
                                            )
)
        {

        }
    }
}
