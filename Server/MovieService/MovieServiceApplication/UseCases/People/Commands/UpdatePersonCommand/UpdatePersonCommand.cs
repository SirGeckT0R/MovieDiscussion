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
    }
}
