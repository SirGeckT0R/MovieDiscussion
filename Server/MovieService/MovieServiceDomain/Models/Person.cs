namespace MovieServiceDomain.Models
{
    public class Person : IdModel
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public DateTime DateOfBirth { get; private set; } = DateTime.UtcNow;
    }
}
