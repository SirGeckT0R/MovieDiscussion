using MediatR;
using MovieServiceApplication.Interfaces.UseCases;

namespace MovieServiceApplication.UseCases.People.Commands.UpdatePersonCommand
{
    public record UpdatePersonCommand : ICommand<Unit>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.UtcNow;

        public UpdatePersonCommand() { }
        public UpdatePersonCommand(Guid id, string firstName, string lastName, DateTime dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }
    }
}
